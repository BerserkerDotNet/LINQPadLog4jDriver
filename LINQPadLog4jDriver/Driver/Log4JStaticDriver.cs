using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BerserkerDotNet.LINQPadLog4jDriver.Domain;
using BerserkerDotNet.LINQPadLog4jDriver.Views;
using LINQPad.Extensibility.DataContext;

namespace BerserkerDotNet.LINQPadLog4jDriver.Driver
{
    public class Log4jStaticDriver: StaticDataContextDriver
    {
        public override string GetConnectionDescription(IConnectionInfo cxInfo)
        {
            return new Log4jConnectionProperties(cxInfo).Name;
        }

        public override bool ShowConnectionDialog(IConnectionInfo cxInfo, bool isNewConnection)
        {
            var editor = new ConnectionStringEditor(cxInfo);
            cxInfo.CustomTypeInfo.CustomTypeName = typeof (Log4jDataContext).FullName;
            return editor.ShowDialog() == true;
        }

        public override string Name
        {
            get { return "Log4j LINQPad Driver"; }
        }

        public override string Author
        {
            get { return "BerserkerDotNet"; }
        }

        public override List<ExplorerItem> GetSchema(IConnectionInfo cxInfo, Type customType)
        {
            var result = new List<ExplorerItem>();
            var logsTable = new ExplorerItem("Logs", ExplorerItemKind.QueryableObject, ExplorerIcon.Table)
                {
                    IsEnumerable = true,DragText = "Logs"
                };
            var properties =
                typeof (LogEntry).GetProperties()
                                 .Select(p => new ExplorerItem(p.Name, ExplorerItemKind.Property, ExplorerIcon.Column))
                                 .ToList();
            logsTable.Children = properties;
            result.Add(logsTable);
            var filesItem = new ExplorerItem("Files", ExplorerItemKind.Category, ExplorerIcon.Box);

            filesItem.Children = DirectoryHelper.GetActiveFiles(new Log4jConnectionProperties(cxInfo))
                     .Select(f => new ExplorerItem(f, ExplorerItemKind.Schema, ExplorerIcon.Schema)).ToList();
            result.Add(filesItem);
            result.Add(new ExplorerItem("ClearCache", ExplorerItemKind.QueryableObject, ExplorerIcon.StoredProc){DragText = "ClearCache()"});
            return result;

        }

        public override ParameterDescriptor[] GetContextConstructorParameters(IConnectionInfo cxInfo)
        {
            return new[] { new ParameterDescriptor("properties", "BerserkerDotNet.LINQPadLog4jDriver.Domain.Log4jConnectionProperties") };
        }

        public override object[] GetContextConstructorArguments(IConnectionInfo cxInfo)
        {
            var properties = new Log4jConnectionProperties(cxInfo);
            return new object[] {properties};

        }

        public override bool AreRepositoriesEquivalent(IConnectionInfo c1, IConnectionInfo c2)
        {
            var f1 = new Log4jConnectionProperties(c1).Folder;
            var f2 = new Log4jConnectionProperties(c2).Folder;
            return f1.Equals(f2, StringComparison.OrdinalIgnoreCase);
        }
    }
}