using System;
using System.Collections.Generic;
using System.Linq;
using EcoHotels.Core.Domain.Models.Property;
using EcoHotels.Core.Infrastructure.Cache;
using EcoHotels.Core.Infrastructure.NH;
using NHibernate.Criterion;

namespace EcoHotels.Core.Infrastructure.Services.Impl.Property
{
    public class OrganizationService : IOrganizationService
    {
        private ICacheStorage CacheStorage;

        private Repository<Organization> OrganizationRepo;

        public OrganizationService(ICacheStorage cacheStorage)
        {
            CacheStorage = cacheStorage;

            OrganizationRepo = new Repository<Organization>();            
        }

        public List<Organization> FindAll()
        {
            return OrganizationRepo.FindAll().ToList();
        }

        public Organization FindById(int id)
        {
            return OrganizationRepo.FindBy(id);
        }

        public Organization FindByHotelId(int hotelId)
        {
            var criteria = DetachedCriteria.For(typeof(Organization))
                .CreateAlias("Hotels", "h")
                    .Add(Restrictions.Eq("h.Id", hotelId));

            return OrganizationRepo.FindOne(criteria);
        }

        public Organization FindSystemOrganization()
        {
            throw new NotImplementedException();
            //return OrganizationRepo.FindBy(Organization.SystemOrganizationId);
        }

        public void Save(Organization organization)
        {
            if (organization.IsValid())
            {

                OrganizationRepo.Save(organization);
            }
        }

        public void Delete(Organization organization)
        {
            //if (organization.Id == Organization.SystemOrganizationId)
            //{
            //    throw new ArgumentException("Can not delete system organization.");
            //}

            OrganizationRepo.Remove(organization);
        }
    }
}
