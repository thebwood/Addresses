using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Addresses.Domain.Models
{
    public class ResultModel
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<ErrorModel> Errors { get; set; }
        public ResultModel()
        {
            Errors = new List<ErrorModel>();
        }

    }

    public class ResultModel<TValue> : ResultModel
    {
        public TValue? Value { get; set; }

        public ResultModel()
        {
            Errors = new List<ErrorModel>();
        }
    }

}
