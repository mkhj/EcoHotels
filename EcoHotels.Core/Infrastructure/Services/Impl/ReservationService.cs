using System;
using System.Collections.Generic;
using EcoHotels.Core.Domain.Models.Commerce;
using EcoHotels.Core.Infrastructure.Cache;
using EcoHotels.Core.Infrastructure.NH;
using NHibernate.Criterion;

namespace EcoHotels.Core.Infrastructure.Services.Impl
{
    public class ReservationService : IReservationService
    {
        private Repository<Reservation> ReservationRepo;

        public ReservationService(ICacheStorage cacheStorage)
        {            
            ReservationRepo = new Repository<Reservation>();
        }

        public Reservation FindBy(int customerId, int reservationId)
        {
            var criteria = DetachedCriteria.For(typeof(Reservation))
                .Add(Restrictions.Eq("Customer.Id", customerId))
                .Add(Restrictions.Eq("Id", reservationId));

            return ReservationRepo.FindOne(criteria);
        }

        public IEnumerable<Reservation> FindBy(int hotelId, DateTime arrival, DateTime departure)
        {
            var criteria = DetachedCriteria.For(typeof(Reservation))
                .Add(Restrictions.Eq("HotelId", hotelId))
                .Add(Restrictions.Ge("Arrival", arrival.Date))
                .Add(Restrictions.Le("Departure", departure.Date));

            return ReservationRepo.FindAll(criteria);
        }

        public IEnumerable<Reservation> FindByCustomer(int id)
        {
            var criteria = DetachedCriteria.For(typeof(Reservation))
                .Add(Restrictions.Eq("Customer.Id", id));

            return ReservationRepo.FindAll(criteria);
        }

        public void Save(Reservation reservation)
        {
            if(reservation.IsValid())
            {
                ReservationRepo.Save(reservation);
            }
        }
    }
}
