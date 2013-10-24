using System.Threading;

namespace BerserkerDotNet.LINQPadLog4jDriver
{
    public static class StringExtensions
    {
        public static string ToTitleCase(this string value)
        {
            var textInfo = Thread.CurrentThread.CurrentCulture.TextInfo;
            var lower = textInfo.ToLower(value);
            return textInfo.ToTitleCase(lower);
        }
    }
}