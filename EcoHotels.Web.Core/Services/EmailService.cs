using System.Net.Mail;
using System.Text;
using EcoHotels.Core.Domain.Models.Commerce;
using EcoHotels.Core.Domain.Models.Property;
using EcoHotels.Core.Domain.Models.Security;
using EcoHotels.Core.Email;

namespace EcoHotels.Web.Core.Services
{
    public class EmailService
    {
        private SendGridEmailService SendGridEmailService;

        public EmailService()
        {
            SendGridEmailService = new SendGridEmailService();
        }

        public void SendReservationEmailToCustomer(Hotel hotel, Reservation reservation)
        {
            dynamic email = SendGridEmailService.CreateEmail("reservation_confirmation_hotel_html");
            email.Hotel = hotel;
            email.Reservation = reservation;

            var message = SendGridEmailService.RenderEmail(email);

            message.From = new MailAddress(hotel.Email);
            message.AddTo(reservation.Email);

            message.Subject = "Your reservation at " + hotel.Name;

            SendGridEmailService.Send(message);
        }

        public void SendPasswordChangedEmailToUser(User user, string password, Hotel hotel)
        {
            dynamic template = SendGridEmailService.CreateEmail("admin_user_password_changed_html");
            template.User = user;
            template.Password = password;
            template.Hotel = hotel;

            var message = SendGridEmailService.RenderEmail(template);

            message.From = new MailAddress(hotel.Email, hotel.Name, Encoding.UTF8);
            message.AddTo(user.Email);

            message.Subject = "Your password has been changed";

            SendGridEmailService.Send(message);
        }

        public void SendAccountCreatedEmailToUser(User user, string password, Hotel hotel)
        {
            dynamic template = SendGridEmailService.CreateEmail("admin_user_account_created_html");
            template.User = user;
            template.Password = password;
            template.Hotel = hotel;

            var message = SendGridEmailService.RenderEmail(template);

            message.From = new MailAddress(hotel.Email, hotel.Name, Encoding.UTF8);
            message.AddTo(user.Email);

            message.Subject = "Your account has been created";

            SendGridEmailService.Send(message);
        }

        public void TestEmailService(string to)
        {
            SendGridEmailService.SendTEST(to, "info@iloveecohotels.com", string.Empty, "Dette er blot en test", "Jeg håber at denne email kommer igennem.");
        }
    }
}
