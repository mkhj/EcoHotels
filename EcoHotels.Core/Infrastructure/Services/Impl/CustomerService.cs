using EcoHotels.Core.Common.Enum;
using EcoHotels.Core.Domain.Models.Commerce;
using EcoHotels.Core.Infrastructure.NH;
using NHibernate.Criterion;

namespace EcoHotels.Core.Infrastructure.Services.Impl
{
    public class CustomerService : ICustomerService
    {
        private readonly Repository<Customer> CustomerRepo;

        public CustomerService()
        {
            CustomerRepo = new Repository<Customer>();
        }

        public Customer FindById(int id)
        {
            return CustomerRepo.FindBy(id);
        }

        public Customer FindByEmail(string email)
        {
            var criteria = DetachedCriteria.For(typeof(Customer))
                .Add(Restrictions.Eq("Email", email.Trim()));

            return CustomerRepo.FindOne(criteria);
        }

        public Customer FindByExternalId(string externalId, AccountTypeEnum accountTypeEnum)
        {
            var criteria = DetachedCriteria.For(typeof(Customer))
                .Add(Restrictions.Eq("ExternalId", externalId.Trim()))
                .Add(Restrictions.Eq("AccountType", accountTypeEnum));

            return CustomerRepo.FindOne(criteria);
        }

        public void Save(Customer entity)
        {
            if(entity.IsValid())
            {
                CustomerRepo.Save(entity);
            }
        }

        public bool IsEmailUnique(string email)
        {
            var criteria = DetachedCriteria.For(typeof(Customer))
                .Add(Restrictions.Eq("Email", email.Trim()));

            return !CustomerRepo.Exists(criteria);
        }
    }
}
