using System.Collections.Generic;
using EcoHotels.Core.Domain.Models.Localization;

namespace EcoHotels.Core.Infrastructure.Services
{
    public interface ILanguageService
    {
        Language FindById(int id);

        Language FindByShortName(string shortname);

        Language FindSystemDefault();

        IEnumerable<Language> FindAll();

        IEnumerable<Language> FindAllActive();

        void Save(Language language);
    }
}
