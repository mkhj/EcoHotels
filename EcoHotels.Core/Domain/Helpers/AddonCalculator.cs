using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EcoHotels.Core.Common;

namespace EcoHotels.Core.Domain.Helpers
{
    internal class AddonCalculator
    {
        public static decimal CalculateAddonPrice(decimal price, CalculationRule calculationRule, PostingRhythm postingRhythm, int adults, int children, int babies, int days)
        {
            var total = 0.0m;

            switch (calculationRule)
            {
                case CalculationRule.PerPerson:
                    total = (adults + children + babies) * price;
                    break;
                case CalculationRule.PerAdult:
                    total = adults * price;
                    break;
                case CalculationRule.PerChild:
                    total = children * price;
                    break;
                case CalculationRule.PerBaby:
                    total = babies * price;
                    break;
                case CalculationRule.PerRoom:
                    total = 1 * price;
                    break;
                default:
                    break;
            }

            switch (postingRhythm)
            {
                case PostingRhythm.Everyday:
                    total = total * days;
                    break;
                case PostingRhythm.ChargeOnce:
                    //NOTE: nothing to do here
                    break;
                default:
                    break;
            }

            return total;
        }
    }
}
