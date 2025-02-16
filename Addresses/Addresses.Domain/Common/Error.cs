namespace Addresses.Domain.Common
{
    public class Error
    {
        private string _code;
        private string _name;
        public Error(string code, string name)

        {
            _code = code;
            _name = name;
        }
        public static Error None = new(string.Empty, string.Empty);
        public static Error NullValue = new("Error.NullValue", "Null value was provided");

    }
}
