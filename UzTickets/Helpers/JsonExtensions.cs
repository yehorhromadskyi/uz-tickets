using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace UzTickets.Helpers
{
    public static class JsonExtensions
    {
        public static T To<T>(this Stream stream)
        {
            var result = default(T);

            try
            {
                using (var streamReader = new StreamReader(stream))
                using (var jsonReader = new JsonTextReader(streamReader))
                {
                    result = new JsonSerializer().Deserialize<T>(jsonReader);
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return result;
        }
    }
}
