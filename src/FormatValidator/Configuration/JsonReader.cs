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
            return Newtonsoft.Json.JsonConvert.DeserializeObject<ValidatorConfiguration>(json);
        }
    }
}
