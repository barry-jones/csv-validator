using System;
using System.Text.RegularExpressions;

namespace FormatValidator
{
    /// <summary>
    /// Seperates a string in to multiple columns based on seperator strings.
    /// </summary>
    internal class ColumnSplitter
    {
        /// <summary>
        /// Splits a <paramref name="input"/> string in to seperate parts based on the provided
        /// <paramref name="seperator"/>.
        /// </summary>
        /// <param name="input">The string to be seperated</param>
        /// <param name="seperator">The seperator string to split on.</param>
        /// <returns>An array of strings</returns>
        public static string[] Split(string input, string seperator)
        {
            return Regex.Split(input,
                $"{EscapeSeperator(seperator)}(?=([^\"]*\"[^\"]*\")*[^\"]*$)",
                RegexOptions.ExplicitCapture
                );
        }

        // escape all of the regex special characters
        private static string EscapeSeperator(string seperator)
        {
            return seperator
                .Replace(@"\", @"\\")
                .Replace("^", @"\^")
                .Replace("$", @"\$")
                .Replace(".", @"\.")
                .Replace("|", @"\|")
                .Replace("?", @"\?")
                .Replace("*", @"\*")
                .Replace("+", @"\+")
                .Replace("{", @"\{")
                .Replace("[", @"\[")
                .Replace("(", @"\(")
                .Replace(")", @"\)");
        }
    }
}
