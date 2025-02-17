namespace Addresses.Domain.Common
{
    public class Error
    {
        public string Code { get; set; }
        public string Name { get; set; }


        public Error(string code, string name)

        {
            Code = code;
            Name = name;
        }
        public static Error None = new(string.Empty, string.Empty);
        public static Error NullValue = new("Error.NullValue", "Null value was provided");

    }
}
