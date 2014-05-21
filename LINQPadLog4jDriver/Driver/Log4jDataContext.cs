using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using BerserkerDotNet.LINQPadLog4jDriver.Domain;
using LINQPad;

namespace BerserkerDotNet.LINQPadLog4jDriver.Driver
{
    public class Log4jDataContext
    {
        private readonly Log4jConnectionProperties _properties;
        private static IList<LogEntry> _logEntriesCache;

        public Log4jDataContext(Log4jConnectionProperties properties)
        {
            _properties = properties;
        }

        private IEnumerable<LogEntry> ReadLogs()
        {
            DirectoryHelper.RefreshFilteredFilesList(_properties);
            var logFiles = DirectoryHelper.GetActiveFiles(_properties);
            var date = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            var settings = new XmlReaderSettings { ConformanceLevel = ConformanceLevel.Fragment, };
            var nameTable = new NameTable();
            var nameSpaceManager = new XmlNamespaceManager(nameTable);
            nameSpaceManager.AddNamespace("log4j", "http://jakarta.apache.org/log4j");

            var parserContext = new XmlParserContext(nameTable, nameSpaceManager,
                                                     null,
                                                     XmlSpace.Default);
            foreach (var logFile in logFiles)
            {
                using (var fileStream = new FileStream(logFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (var sReader = new StreamReader(fileStream, Encoding.Default, true))
                using (var xmlTextReader = XmlReader.Create(sReader, settings, parserContext))
                    foreach (var logEntry in ProcessLogEntry(xmlTextReader, logFile, date))
                        yield return logEntry;
            }
        }

        private static IEnumerable<LogEntry> ProcessLogEntry(XmlReader xmlTextReader, string logFile, DateTime date)
        {
            DateTime? prevTimeStamp = null;
            while (xmlTextReader.Read())
            {
                if ((xmlTextReader.NodeType != XmlNodeType.Element) || (xmlTextReader.Name != "log4j:event"))
                    continue;

                var entry = new LogEntry();
                entry.Path = logFile;
                entry.Logger = xmlTextReader.GetAttribute("logger");
                entry.TimeStamp = date.AddMilliseconds(Convert.ToDouble(xmlTextReader.GetAttribute("timestamp"))).ToLocalTime();

                if (prevTimeStamp.HasValue)
                    entry.Delta = (entry.TimeStamp - prevTimeStamp.Value).TotalSeconds;
                prevTimeStamp = entry.TimeStamp;

                var severityLevel = xmlTextReader.GetAttribute("level");
                entry.SeverityLevel = severityLevel;
                entry.Thread = xmlTextReader.GetAttribute("thread");

                ProcessChildNodes(xmlTextReader, entry);
                yield return entry;
            }
        }

        private static void ProcessChildNodes(XmlReader xmlTextReader, LogEntry entry)
        {
            while (xmlTextReader.Read())
            {
                var hasNoMoreChildToProcess = ProcessChildNode(xmlTextReader, entry);
                if (hasNoMoreChildToProcess)
                    break;
            }
        }

        private static bool ProcessChildNode(XmlReader xmlTextReader, LogEntry entry)
        {
            switch (xmlTextReader.Name)
            {
                case "log4j:event":
                    return true;
                case ("log4j:message"):
                    entry.Message = xmlTextReader.ReadString();
                    break;
                case ("log4j:data"):
                    ProcessDataTag(xmlTextReader, entry);
                    break;
                case ("log4j:throwable"):
                    entry.Throwable = xmlTextReader.ReadString();
                    break;
                case ("log4j:locationInfo"):
                    SetLocationInfo(entry, xmlTextReader);
                    break;
            }
            return false;
        }

        private static void SetLocationInfo(LogEntry entry, XmlReader xmlTextReader)
        {
            entry.Class = xmlTextReader.GetAttribute("class");
            entry.Method = xmlTextReader.GetAttribute("method");
            entry.File = xmlTextReader.GetAttribute("file");
            entry.Line = xmlTextReader.GetAttribute("line");
        }

        private static void ProcessDataTag(XmlReader xmlTextReader, LogEntry entry)
        {
            switch (xmlTextReader.GetAttribute("name"))
            {
                case ("log4net:UserName"):
                    entry.UserName = xmlTextReader.GetAttribute("value");
                    break;
                case ("log4japp"):
                    entry.App = xmlTextReader.GetAttribute("value");
                    break;
                case ("log4jmachinename"):
                    entry.MachineName = xmlTextReader.GetAttribute("value");
                    break;
                case ("log4net:HostName"):
                    entry.HostName = xmlTextReader.GetAttribute("value");
                    break;
            }
        }

        public IEnumerable<LogEntry> Logs
        {
            get
            {
                if (_properties.UseCache)
                {
                    return _logEntriesCache ?? (_logEntriesCache = ReadLogs().ToList());
                }
                return ReadLogs();
            }
        }

        public string ClearCache()
        {
            if (_logEntriesCache == null)
                return "No cache to clear!";

            _logEntriesCache.Clear();
            _logEntriesCache = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();

            return "Done";
        }
    }
}