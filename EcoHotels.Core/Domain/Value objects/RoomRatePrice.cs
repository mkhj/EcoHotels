using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcoHotels.Core.Domain.Value_objects
{
    [Serializable]
    public class RoomRatePrice
    {
        private int discount;
        private double value;

        public RoomRatePrice(Guid roomId, DateTime date, double value)
        {
            RoomId = roomId;
            this.Date = date;
            this.value = value;
        }

        public Guid RoomId { get; private set; }

        public DateTime Date { get; private set; }

        public double Value
        {
            get { return value * (discount / 100.0); }
        }

        public void ApplyDiscount(int percentage)
        {
            discount = percentage;
        }

    }
}
