using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EcoHotels.Core.Domain.Models.Localization;

namespace EcoHotels.Core.Infrastructure.Repositories.NH
{
    public class Ip2LocationRepo : Repository<Ip2Location>
    {
        public string FindIP2Location(float ipx)
        {
            return Session.CreateSQLQuery("SELECT ip.countrySHORT FROM IP2Location ip WHERE ipFROM <= :IPx AND :IPx <= ipTO")
                    .SetDouble("IPx", ipx)
                    .SetMaxResults(1)
                    .UniqueResult<string>();

            //return Session.CreateSQLQuery("SELECT ip.countrySHORT FROM IP2Location ip WHERE ipFROM <= :IPx AND :IPx <= ipTO")
            //    .SetDouble("IPx", ipx)
            //    .SetMaxResults(1)
            //    .List<string>()
            //    .FirstOrDefault();

            //var location = Session.CreateQuery("FROM IP2Location ip WHERE ipFROM <= :IPx AND :IPx <= ipTO")
            //                .SetDouble("IPx", ipx)
            //                .UniqueResult<Ip2Location>();

            //return (location == null) ? string.Empty : location.CountryShortName;

            //var location = (IP2Location)ActiveRecordMediator.Execute(typeof(IP2Location),
            //                                    delegate(ISession session, object instance)
            //                                    {
            //                                        //[countryLONG], [countrySHORT], [ipFROM], [ipTO]
            //                                        return session.CreateQuery("FROM IP2Location ip WHERE ipFROM <= :IPx AND :IPx <= ipTO")
            //                                            .SetDouble("IPx", ipx)
            //                                            //.SetResultTransformer(new DistinctRootEntityResultTransformer())
            //                                            .List<IP2Location>()
            //                                            .FirstOrDefault();
            //                                    }, null);

            //return (location == null) ? string.Empty : location.CountryNameShort;
        }
    }
}
