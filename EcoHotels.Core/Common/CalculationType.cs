using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcoHotels.Core.Common
{
    public enum CalculationRule
    {
        PerPerson = 1,

        PerAdult = 2,

        PerChild = 3,

        PerBaby = 4,

        PerRoom = 5
    }

    public enum PostingRhythm
    {
        Everyday = 1,

        ChargeOnce = 5
    }
}
