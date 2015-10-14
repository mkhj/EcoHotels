using System;

namespace EcoHotels.Extensions
{
    public static class IntegerExtensions
    {
        public static T ConvertToEnum<T>(this int value)
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }

            return (T)Enum.ToObject(typeof(T), value);
        }
    }
}
