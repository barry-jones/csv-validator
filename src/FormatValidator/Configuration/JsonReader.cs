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
            ValidatorConfiguration configuration = new ValidatorConfiguration();

            if (!string.IsNullOrEmpty(json))
            {
                configuration = Newtonsoft.Json.JsonConvert.DeserializeObject<ValidatorConfiguration>(json);
            }

            return configuration;
        }
    }
}
