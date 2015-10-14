using System;
using EcoHotels.Core.Domain.Models.Localization;
using EcoHotels.Core.Infrastructure.NH;
using EcoHotels.Extensions;

namespace EcoHotels.Core.Infrastructure.Services.Impl
{
    public class IP2LocationService : IIP2LocationService
    {
        private Repository<Ip2Location> Repo;

        public IP2LocationService()
        {
            Repo = new Repository<Ip2Location>();
        }

        public string GetLocation(string ip)
        {
            var IPx = GetIPx(ip);
            if (IPx > 0)
            {
                var location = Repo.Session.CreateSQLQuery("SELECT ip.countrySHORT FROM IP2Location ip WHERE ipFROM <= :IPx AND :IPx <= ipTO")
                        .SetDouble("IPx", IPx)
                        .SetMaxResults(1)
                        .UniqueResult<string>();

                return (location.IsNotNull()) ? location : string.Empty;
            }

            return string.Empty;
        }

        private static Single GetIPx(string IP)
        {
            var arIP = IP.Split('.');
            Single IPx = 0;
            for (int i = 0; i < arIP.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        IPx = Convert.ToSingle(arIP[i]) * (256 * 256 * 256);
                        break;
                    case 1:
                        IPx = IPx + (Convert.ToSingle(arIP[i]) * (256 * 256));
                        break;
                    case 2:
                        IPx = IPx + (Convert.ToSingle(arIP[i]) * (256));
                        break;
                    case 3:
                        IPx = IPx + Convert.ToSingle(arIP[i]);
                        break;
                }
            }
            return IPx;
        }
    }
}
