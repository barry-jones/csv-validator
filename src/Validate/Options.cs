using CommandLine;
using CommandLine.Text;
using System;
using System.Collections.Generic;
using System.Text;

namespace Validate
{
    internal class Options
    {
        [Option('f', "file", Required = true, HelpText = "The file to validate")]
        public string File { get; set; }

        [Option('w', "with", Required =true, HelpText = "The configuration file that details the validation")]
        public string With { get; set; }

        [Usage(ApplicationAlias = "validate.exe")]
        public static IEnumerable<Example> Examples
        {
            get
            {
                return new List<Example>()
                {
                    new Example("An example", new Options { File = "myfile.csv", With = "validation-config.json"})
                };
            }
        }
    }
}
