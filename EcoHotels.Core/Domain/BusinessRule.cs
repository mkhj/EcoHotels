using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EcoHotels.Core.Common;

namespace EcoHotels.Core.Domain
{
    [Serializable]
    public class BusinessRule
    {
        public BusinessRule(string property, string rule)
        {
            Check.Require(!string.IsNullOrEmpty(property), "property cat not be empty or null");
            Check.Require(!string.IsNullOrEmpty(rule), "rule cat not be empty or null");

            Property = property;
            Rule = rule;
        }

        public string Property
        {
            get;
            private set;
        }

        public string Rule
        {
            get;
            private set;
        }
    }
}
