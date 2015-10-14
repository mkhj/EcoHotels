using EcoHotels.Core.Domain.Models.Security;
using EcoHotels.Core.Infrastructure.Cache;
using EcoHotels.Core.Infrastructure.NH;
using NHibernate.Criterion;

namespace EcoHotels.Core.Infrastructure.Services.Impl
{
    public class UserService : IUserService
    {
        private ICacheStorage CacheStorage;

        private Repository<User> UserRepo;

        public UserService(ICacheStorage cacheStorage)
        {            
            CacheStorage = cacheStorage;

            UserRepo = new Repository<User>();
        }

        public User FindById(int userId)
        {
            return UserRepo.FindBy(userId);
        }

        public User FindByEmail(string email)
        {
            var criteria = DetachedCriteria.For(typeof(User))
                .Add(Restrictions.Eq("Email", email.Trim()));

            return UserRepo.FindOne(criteria);
        }

        public User FindById(int organizationId, int id)
        {
            var criteria = DetachedCriteria.For(typeof(User))
                .Add(Restrictions.Eq("Id", id))
                .Add(Restrictions.Eq("Organization.Id", organizationId));

            return UserRepo.FindOne(criteria);
        }

        public void Save(User user)
        {
            if (user.IsValid())
            {
                UserRepo.Save(user);
            }
        }

        public void Delete(User user)
        {
            UserRepo.Remove(user);
        }

        public bool IsEmailUnique(string email)
        {
            var criteria = DetachedCriteria.For(typeof(User))
                .Add(Restrictions.Eq("Email", email.Trim()));

            return !UserRepo.Exists(criteria);
        }

        public bool IsEmailUnique(int organizationId, int userId, string email)
        {
            var user = FindById(organizationId, userId);
            if(user == null)
            {
                return true;
            }

            return user.Id == userId;
        }
    }
}
