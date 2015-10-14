namespace EcoHotels.Web.Core.Models
{
    public class JsonResultSuccess
    {
        public JsonResultSuccess(string message)
        {
            status = "success";
            this.message = message;
        }

        public JsonResultSuccess(string message, object data) : this(message)
        {
            this.data = data;
        }

        public string status { get; private set; }

        public string message { get; private set; }

        public object data { get; set; }
    }

    public class JsonResultWarning
    {
        public JsonResultWarning(string message)
        {
            status = "warning";
            this.message = message;
        }

        public string status { get; private set; }

        public string message { get; private set; }
    }

    public class JsonResultError
    {
        public JsonResultError(string message)
        {
            status = "error";
            this.message = message;
        }

        public string status { get; private set; }

        public string message { get; private set; }
    }
}
