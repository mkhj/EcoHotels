using System.Collections.Generic;
using EcoHotels.Core.Domain.Models.Commerce;
using EcoHotels.Core.Domain.Models.Property;

namespace EcoHotels.Core.Infrastructure.Services
{
    public interface IInventoryService
    {
        IEnumerable<RoomTypeInventory> EnsureInventories(Hotel hotel, IEnumerable<RoomTypeInventory> inventories, int year, int month);

        IEnumerable<RoomTypeInventory> FindBy(int roomtypeId, int year, int month);

        IEnumerable<RoomTypeInventory> FindByIds(IEnumerable<int> ids, int year, int month);

        IEnumerable<RoomTypeInventory> FindByDate(int year, int month);

        void Save(RoomTypeInventory inventories);

        void Save(List<RoomTypeInventory> inventories);
    }
}
