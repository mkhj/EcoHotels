using System.Collections.Generic;
using Newtonsoft.Json;

namespace EcoHotels.Extensions
{
    public static class EnumerableExtension
    {
        public static string ToJSON<T>(this IEnumerable<T> array)
        {
            return JsonConvert.SerializeObject(array);
        }
    }
}
