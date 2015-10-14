using System.Collections.Generic;
using EcoHotels.Core.Domain.Models.Localization;

namespace EcoHotels.Core.Infrastructure.Services
{
    public interface IPageInformationService
    {
        PageInformation FindByFilename(string filename);

        IEnumerable<PageInformation> FindAll();

        bool IsFilenameUnique(string filename);
    }
}
