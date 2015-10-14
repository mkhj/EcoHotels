using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web.Mvc;
using EcoHotels.Core.Common;
using EcoHotels.Extensions;
using Postal;
using SendGridMail;
using SendGridMail.Transport;

namespace EcoHotels.Core.Email
{
    public interface IEmailService
    {
        dynamic CreateEmail(string view);

        void Send(string to, string subject, dynamic model);
    }

    public class SendGridEmailService : IEmailService
    {
        private IEmailViewRenderer EmailViewRenderer { get; set; }
        private SMTP TransportInstance { get; set; }

        public SendGridEmailService()
        {
            TransportInstance = SMTP.GenerateInstance(new NetworkCredential("....", "...."), "smtp.sendgrid.net", Convert.ToInt32(587));
        }

        public dynamic CreateEmail(string view)
        {
            return new Postal.Email(view);
        }


        /// <summary>
        /// Send a simple HTML based email
        /// </summary>
        private void Send(Postal.Email email)
        {


            ////create a new message object
            //var message = SendGrid.GenerateInstance();

            //message.AddTo(to);

            ////set the sender
            //message.From = new MailAddress(from);

            ////set the message body
            //message.Html = CreateHtmlMailFromView(view); // "<html><p>Hello</p><p>World</p></html>";

            //message.Text = "sdfsdfsdf";

            ////set the message subject
            //message.Subject = "Hello World HTML Test";

            ////enable Open Tracking
            //message.EnableOpenTracking();

            ////enable clicktracking
            //message.EnableClickTracking(false);

            ////enable spamcheck
            //message.EnableSpamCheck();

            ////create an instance of the SMTP transport mechanism
            //var transportInstance = SMTP.GenerateInstance(new NetworkCredential("username", "password"));

            ////send the mail
            //transportInstance.Deliver(message);
        }

        public SendGrid RenderEmail(dynamic model)
        {
            //create a new message object
            var message = SendGrid.GenerateInstance();

            message.Html = EmailViewRenderer.Render(model);
            //message.Text = EmailViewRenderer.Render(text);

            return message;
        }

        public void Send(SendGrid message)
        {
            //enable Open Tracking
            message.EnableOpenTracking();

            //enable clicktracking
            message.EnableClickTracking(false);

            //enable spamcheck
            // http://docs.sendgrid.com/documentation/apps/spam-checker-filter/
            //message.EnableSpamCheck();

            TransportInstance.Deliver(message);
        }

        public void Send(string to, string subject, dynamic template)
        {
            Check.Assert(to.IsNotNull(), "Email To can not be null or empty.");
            Check.Assert(!string.IsNullOrEmpty(subject), "Subject can not be null or empty.");

            var message = RenderEmail(template);
            message.From = new MailAddress("no-reply@iloveecohotels.com", "I Love Eco Hotels", Encoding.UTF8);
            message.AddTo(to);
            message.Subject = subject;
            
            Send(message);
        }

        
        public void SendTEST(string to, string from, string bcc, string subject, string body)
        {
            var message = SendGrid.GenerateInstance();

            message.From = new MailAddress(from);
            message.AddTo(to);

            message.Subject = subject;
            message.Text = body;

            message.InitializeFilters();

            message.EnableOpenTracking();

            message.EnableClickTracking(true);


            TransportInstance.Deliver(message);
        }
    }
}
