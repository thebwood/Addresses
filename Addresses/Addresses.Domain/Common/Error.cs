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

    }
}
