using System;
using Nancy;
using Newtonsoft.Json;

namespace HdbApi
{
    class ResponseHandler
    {
        public Response BuildErrorJson(string errorMessage, HttpStatusCode sCode)
        {
            var error = new ErrorReponse();
            error.status = (int)sCode;
            error.message = errorMessage;
            error.requesttime = DateTime.Now;

            var response = (Response)JsonConvert.SerializeObject(error);
            response.StatusCode = sCode;
            response.ContentType = "application/json";
            return response;
        }

        public class ErrorReponse
        {
            public int status { get; set; }
            public string message { get; set; }
            public DateTime requesttime { get; set; }
        }
    }
}
