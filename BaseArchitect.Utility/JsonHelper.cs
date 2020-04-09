using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace BaseArchitect.Utility
{
    public static class JsonHelper
    {
        public static string ToJson(this object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static T ParseAs<T>(this string jsonString)
        {
            return JsonConvert.DeserializeObject<T>(jsonString);
        }

        public static T ReadAs<T>(this HttpContent content)
        {
            using (var stream = content.ReadAsStreamAsync().Result)
            using (var sr = new StreamReader(stream))
            using (var reader = new JsonTextReader(sr))
            {
                var serializer = new JsonSerializer();
                return serializer.Deserialize<T>(reader);
            }
        }

        public static StringContent ToJsonStringContent(this object obj)
        {
            return new StringContent(obj.ToJson(), Encoding.UTF8, "application/json");
        }

        public static Dictionary<string, object> ToDictionary<TInput>(TInput obj)
        {
            if (obj == null) return new Dictionary<string, object>();
            return typeof(TInput).GetProperties()
                .ToDictionary(p => p.Name, p => p.GetValue(obj, null));
        }
    }
}
