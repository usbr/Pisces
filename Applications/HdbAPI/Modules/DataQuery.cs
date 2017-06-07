using System;
using System.Collections.Generic;
using Nancy;
using Nancy.ErrorHandling;
using Nancy.ModelBinding;

namespace HdbApi.Modules.DataQuery
{
    public class DataQuery : NancyModule
    {
        private string response;

        public DataQuery()
        {
            Get["/tsquery"] = args =>
            {
                // Bind inputs to a DataQueryRequest object
                var queryArgs = this.Bind<DataQueryRequest>();
                // Validate DataQueryRequest object
                var validation = Validate(queryArgs);
                if (validation.IsValid)
                {
                    return response = "";
                }
                else
                {
                    var e = new ResponseHandler();
                    var response = e.BuildErrorJson(validation.ValidationMessage, HttpStatusCode.BadRequest);
                    return response;
                }
            };

        }

        private Validation.ValidationResult Validate(DataQueryRequest request)
        {
            var validator = new Validation.Validator();
            var validation = new Validation.ValidationResult();
            validator.ValidateInput("format", request.format, typeof(string), validation);
            validator.ValidateInput("sdi", request.sdi, "allnumbers", validation);
            validator.ValidateInput("svr", request.svr, typeof(string), validation);
            validator.ValidateInput("t1", request.t1, typeof(DateTime), validation);
            validator.ValidateInput("t2", request.t2, typeof(DateTime), validation);
            validator.ValidateInput("tstp", request.tstp, new List<string> { "HR", "DY", "MN", "YR" }, validation);
            return validation;
        }

        public class DataQueryRequest
        {
            // Legacy HDB CGI program call
            // http://ibr3lcrsrv01.bor.doi.net:8080/HDB_CGI.com?svr=lchdb2&sdi=1863,1930,2166,2146&tstp=DY&t1=1/1/1980&t2=1/1/2016&table=R&mrid=-1&format=9
            public string svr { get; set; }
            public string sdi { get; set; }
            public string tstp { get; set; }
            public string t1 { get; set; }
            public string t2 { get; set; }
            public string table { get; set; }
            public string mrid { get; set; }
            public string format { get; set; }
        }


    }
}