using EcoHotels.Core.Common.Enum;
using EcoHotels.Core.Domain.Models.Commerce;

namespace EcoHotels.Core.Infrastructure.Services
{
    public interface ICustomerService
    {
        Customer FindById(int id);

        Customer FindByEmail(string email);

        Customer FindByExternalId(string externalId, AccountTypeEnum accountTypeEnum);

        void Save(Customer entity);

        bool IsEmailUnique(string email);
    }
}
