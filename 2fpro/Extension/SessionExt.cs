using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace _2fpro.Extension
{
    public static class SessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);

            return value == null ? default(T) :
                JsonConvert.DeserializeObject<T>(value);
        }

        public static void Set<T>(this IDistributedCache session, string key, T value)
        {

            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T Get<T>(this IDistributedCache session, string key)
        {
            var value = session.GetString(key);

            return value == null ? default(T) :
                JsonConvert.DeserializeObject<T>(value);
        }

        public static async Task SetAsync<T>(this IDistributedCache session, string key, T value)
        {
            await session.SetStringAsync(key, JsonConvert.SerializeObject(value));

        }

        public static async Task<T> GetAsync<T>(this IDistributedCache session, string key)
        {
            var value = await session.GetStringAsync(key);

            return value == null ? default(T) :
                JsonConvert.DeserializeObject<T>(value);
        }
    }
}
