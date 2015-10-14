using EcoHotels.Core.Domain.Models.Security;

namespace EcoHotels.Core.Infrastructure.Services
{
    public interface IUserService
    {
        User FindById(int id);

        User FindById(int organizationId, int id);

        User FindByEmail(string email);

        void Save(User entity);

        void Delete(User user);

        bool IsEmailUnique(string email);

        bool IsEmailUnique(int organizationId, int userId, string email);
    }
}
