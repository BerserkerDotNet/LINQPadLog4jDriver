using System.Xml.Linq;
using LINQPad.Extensibility.DataContext;

namespace BerserkerDotNet.LINQPadLog4jDriver.Domain
{
    public class Log4jConnectionProperties
    {
        private const string PATH_KEY = "Path";
        private const string CACHE_KEY = "UseCache";
        private const string FILE_NAME_FILTER_KEY = "FileNameFilter";

        private readonly XElement _driverData;

        public Log4jConnectionProperties(IConnectionInfo cxInfo)
        {
            _driverData = cxInfo.DriverData;
        }

        public string Folder
        {
            get { return (string)_driverData.Element(PATH_KEY) ?? string.Empty; }
            set { _driverData.SetElementValue(PATH_KEY, value); }
        }

        public bool UseCache
        {
            get { return bool.Parse((string)_driverData.Element(CACHE_KEY) ?? "false"); }
            set { _driverData.SetElementValue(CACHE_KEY, value); }
        }

        public string Name {
            get { return Folder.Substring(Folder.LastIndexOf('\\') + 1); }
        }

        public string FileNameFilter
        {
            get { return (string)_driverData.Element(FILE_NAME_FILTER_KEY) ?? string.Empty; }
            set { _driverData.SetElementValue(FILE_NAME_FILTER_KEY, value); }
        }
    }
}