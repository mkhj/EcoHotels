using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace EcoHotels.Web.Core.Social
{
    [DataContract]
    public class FacebookData
    {
        [DataMember(Name = "credentials")]
        public FacebookCredentials Credentials { get; set; }

        [DataMember(Name = "user")]
        public FacebookUserdata User { get; set; }        
    }

    /// <summary>
    /// </summary>
    [DataContract]
    public class FacebookUserdata
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }

        public string First_name { get; set; }

        public string Last_name { get; set; }

        public string Gender { get; set; }

        public string Birthday { get; set; }

        public string Link { get; set; }

        public DateTime ConvertBirthdayToDatetime()
        {
            var date = DateTime.MinValue;
            var isValid = DateTime.TryParse(Birthday, CultureInfo.CreateSpecificCulture("en-US"), DateTimeStyles.None, out date);
            return date;
        }

        public bool HasValidBirthday
        {
            get { return ConvertBirthdayToDatetime() != DateTime.MinValue; }
        }

        public string PictureUrl
        {
            get { return string.Format("http://graph.facebook.com/{0}/picture", Id); }
        }
    }

    [DataContract]
    public class FacebookCredentials
    {
        [DataMember(Name = "accessToken")]
        public string AccessToken { get; set; }

        [DataMember(Name = "uid")]
        public string UserId { get; set; }
    }
}
