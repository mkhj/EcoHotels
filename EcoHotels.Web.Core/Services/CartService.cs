using System;
using System.Web;
using EcoHotels.Core.Domain.Models.Commerce;
using EcoHotels.Core.Domain.Value_objects;

namespace EcoHotels.Web.Core.Services
{
    public interface ICartService
    {
        void Remove();

        void Update(Reservation reservation);

        Reservation GetCartContent();

        bool HasActiveCart();
    }

    public class CartService : ICartService
    {
        private const string SESSION_KEY = "EcoHotels.Cart";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cartId"></param>
        /// <param name="ipaddress"></param>
        /// <returns></returns>
        private string CreateSessionKey()
        {
            return string.Concat(SESSION_KEY);
        }

        //public Cart Create(Currency currency, string ipaddress)
        //{
        //    var cart = new Cart(ipaddress, currency);
        //    var sessionKey = CreateSessionKey();
        //    HttpContext.Current.Session[sessionKey] = cart;

        //    return cart;
        //}

        public void Update(Reservation reservation)
        {
            var sessionKey = CreateSessionKey();
            HttpContext.Current.Session[sessionKey] = reservation;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public void Remove()
        {
            var sessionKey = CreateSessionKey();
            HttpContext.Current.Session.Remove(sessionKey);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Reservation GetCartContent()
        {
            var sessionKey = CreateSessionKey();
            var cart = HttpContext.Current.Session[sessionKey] as Reservation;
            return cart;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool HasActiveCart()
        {
            return GetCartContent() != null;
        }

    }
}
