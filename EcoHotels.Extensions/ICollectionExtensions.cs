using System;
using System.Collections.Generic;
using System.Linq;

namespace EcoHotels.Extensions
{
    public static class CollectionExtensions
    {
        public static void Remove<T>(this ICollection<T> list, Func<T, bool> predicate)
        {
            var items = list.Where(predicate).ToList();

            foreach (var item in items)
            {
                list.Remove(item);
            }
        }
    }
}
