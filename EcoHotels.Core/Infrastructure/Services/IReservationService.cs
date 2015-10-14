using System;
using System.Collections.Generic;
using EcoHotels.Core.Domain.Models.Commerce;

namespace EcoHotels.Core.Infrastructure.Services
{
    public interface IReservationService
    {
        Reservation FindBy(int customerId, int reservationId);

        IEnumerable<Reservation> FindByCustomer(int id);

        IEnumerable<Reservation> FindBy(int hotelId, DateTime arrival, DateTime departure);

        void Save(Reservation reservation);
    }
}
