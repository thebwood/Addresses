using System.Net;

namespace Addresses.Domain.Common
{
    public class Result
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool Success { get { return Errors.Count == 0; } }
        public string Message { get; set; }
        public List<Error> Errors { get; set; } = new();
    }

    public class Result<TValue> : Result
    {
        private TValue? _value;
        public Result()
        {
            _value = default;
            Errors = new(); 
        }
        public TValue Value
        {
            get
            {
                if (_value == null)
                {
                    throw new InvalidOperationException("Value is null");
                }
                return _value;
            }
            set
            {
                if (value == null)
                {
                    throw new InvalidOperationException("Value is null");
                }
                _value = value;
            }
        }
    }
}