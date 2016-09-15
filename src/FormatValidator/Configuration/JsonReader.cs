using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FormatValidator.Configuration
{
    public class JsonReader : IReader
    {
        public ValidatorConfiguration Read(string json)
        {
            ValidatorConfiguration configuration = null;

            if (string.IsNullOrEmpty(json))
            {
                configuration = new ValidatorConfiguration();
            }
            else
            {
                configuration = Newtonsoft.Json.JsonConvert.DeserializeObject<ValidatorConfiguration>(json);
            }

            return configuration;
        }
    }
}
