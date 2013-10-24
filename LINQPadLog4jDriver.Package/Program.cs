using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Ionic.Zip;

namespace BerserkerDotNet.LINQPadLog4jDriver.Package
{
    class Program
    {
        static void Main(string[] args)
        {
            const string releaseDir = @"";
            var assemblyPath = Path.Combine(Path.Combine(Environment.CurrentDirectory, releaseDir), "LINQPadLog4jDriver.dll");
            var assembly = Assembly.LoadFile(assemblyPath);
            var version = assembly.GetName().Version;

            using (var ms = new MemoryStream())
            using (var releaseZip = new ZipFile())
            using (var lpx45 = new ZipFile())
            {
                var releaseZipPath = Path.Combine(releaseDir, string.Format("Log4jLinqpadDriver {0}.zip", version));
                lpx45.AddFile(Path.Combine(Environment.CurrentDirectory, "LINQPadLog4jDriver.dll"), "");
                lpx45.AddFile(Path.Combine(Environment.CurrentDirectory, "header.xml"), "");
                lpx45.Save(ms);
                ms.Seek(0, SeekOrigin.Begin);
                releaseZip.AddEntry("Log4jLinqpadDriver.lpx", ms);

                releaseZip.Save(releaseZipPath);
                ms.SetLength(0);

                // readme
                releaseZip.AddFile(Path.Combine(releaseDir, "readme.txt"), "");
                releaseZip.Save(releaseZipPath);
            }

            // open
            Process.Start(Environment.CurrentDirectory);
        }
    }
}
