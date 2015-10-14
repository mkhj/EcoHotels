using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcoHotels.Core.Common.Enum
{
    public enum HotelCategoryEnum
    {
        None = 0,
        Adventure = 1,
        AllInclusive = 2,
        ArtAndCulture = 3,
        ByTheBeach = 4,
        BigCityEscapes = 5,
        Casino = 6,
        Classic = 7,
        Countryside = 8,
        City = 9,
        CuttingEdge = 10,
        Design = 11,
        Elegant = 12,
        Entertainment = 13,
        FamilyFriendly = 14,
        FoodAndWine = 15,
        Golf = 16,
        Hip = 17,
        Modern = 18,
        Mountains = 19,
        Pool = 20,
        Relaxed = 21,
        Restaurant = 22,
        Romantic = 23,
        Rustic = 24,
        Shopping = 25,
        Singles = 26,
        SpaAndWellness = 27,
        WaterSports = 28,
        Yoga = 29
    }

    public class HotelCategories
    {
        public static Dictionary<int, string> GetList()
        {
            return new Dictionary<int, string>
                           {
                               {1, "Adventure"},
                               {2, "All-Inclusive"},
                               {3, "Art & Culture"},
                               {4, "By the Beach"},
                               {5, "Big City Escapes"},
                               {6, "Casino"},
                               {7, "Classic"},
                               {8, "Countryside"},
                               {9, "City"},
                               {10, "Cutting-edge"},
                               {11, "Design"},
                               {12, "Elegant"},
                               {13, "Entertainment"},
                               {14, "Family-Friendly"},
                               {15, "Food & Wine"},
                               {16, "Golf"},
                               {17, "Hip"},
                               {18, "Modern"},
                               {19, "Mountains"},
                               {20, "Pool"},
                               {21, "Relaxed"},
                               {22, "Restaurant"},
                               {23, "Romantic"},
                               {24, "Rustic"},
                               {25, "Shopping"},
                               {26, "Singles"},
                               {27, "Spa & Wellness"},
                               {28, "Water Sports"},
                               {29, "Yoga"}
                           };
        }
    }
}
