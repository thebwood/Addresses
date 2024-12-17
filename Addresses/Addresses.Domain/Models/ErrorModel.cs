namespace Addresses.Domain.Models
{
    public class ErrorModel
    {
        public ErrorModel() { }
        public ErrorModel(string message)
        {
            Message = message;
        }

        public string Message { get; set; }
        public string ErrorType { get; set; }

    }
}
