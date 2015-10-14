using System.Collections.Generic;
using EcoHotels.Core.Domain.Models.Property;

namespace EcoHotels.Core.Infrastructure.Services
{
    public interface IOrganizationService
    {
        List<Organization> FindAll();

        Organization FindById(int organizationId);

        Organization FindByHotelId(int hotelId);

        Organization FindSystemOrganization();

        void Save(Organization organization);

        void Delete(Organization organization);
    }
}