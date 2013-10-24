using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using BerserkerDotNet.LINQPadLog4jDriver.Domain;

namespace BerserkerDotNet.LINQPadLog4jDriver.Driver
{
    public static class DirectoryHelper
    {
        public static IEnumerable<string> GetFilteredFiles(Log4jConnectionProperties properties)
        {
            if (string.IsNullOrEmpty(properties.FileNameFilter))
                return Directory.GetFiles(properties.Folder);

            var filter = new Regex(properties.FileNameFilter);
            return Directory.GetFiles(properties.Folder).Where(f => filter.Match(f).Success);
        }
    }
}