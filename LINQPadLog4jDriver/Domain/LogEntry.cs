using System;

namespace BerserkerDotNet.LINQPadLog4jDriver.Domain
{
    public class LogEntry
    {
        public DateTime TimeStamp { get; set; }
        public double? Delta { get; set; }
        public string Logger { get; set; }
        public string Thread { get; set; }
        public string Message { get; set; }
        public string MachineName { get; set; }
        public string UserName { get; set; }
        public string HostName { get; set; }
        public string App { get; set; }
        public string Throwable { get; set; }
        public string Class { get; set; }
        public string Method { get; set; }
        public string File { get; set; }
        public string Line { get; set; }
        public string Uncategorized { get; set; }
        public SeverityLevel SeverityLevel { get; set; }

        public string Path { get; set; }
    }
}