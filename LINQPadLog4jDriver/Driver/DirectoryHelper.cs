using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using BerserkerDotNet.LINQPadLog4jDriver.Domain;

namespace BerserkerDotNet.LINQPadLog4jDriver.Driver
{
    public static class DirectoryHelper
    {
        private static IEnumerable<string> _activeFiles = null;

        public static void RefreshFilteredFilesList(Log4jConnectionProperties properties)
        {
            var folders = properties.Folder.Split(new[] {';'}, StringSplitOptions.RemoveEmptyEntries);

            var getFilesQuery = folders.AsParallel()
                .Where(Directory.Exists)
                .SelectMany(Directory.GetFiles);

            if (!string.IsNullOrEmpty(properties.FileNameFilter))
            {
                var filter = new Regex(properties.FileNameFilter);
                getFilesQuery = getFilesQuery
                    .Where(f => filter.Match(f).Success);
            }

            var dateFilterMode = properties.DateFilterMode;
            if (dateFilterMode != DateFilterMode.None)
            {
                DateTime filterDate;
                if (dateFilterMode == DateFilterMode.SpecificDate)
                    filterDate = properties.SpecificDate.Value;
                else
                {
                    var daysToSubstract = dateFilterMode == DateFilterMode.LastDay
                        ? 1
                        : dateFilterMode == DateFilterMode.PreviousTwoDays ? 2 : 7;
                    filterDate = DateTime.Today.AddDays(-daysToSubstract);
                }

                getFilesQuery = getFilesQuery.Where(f => File.GetLastWriteTimeUtc(f) > filterDate.ToUniversalTime());

            }
            _activeFiles = getFilesQuery.ToList();
        }

        public static IEnumerable<string> GetActiveFiles(Log4jConnectionProperties properties)
        {
            if (_activeFiles == null || !_activeFiles.Any())
                RefreshFilteredFilesList(properties);
            return _activeFiles;

        }
    }
}