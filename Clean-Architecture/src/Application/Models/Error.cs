namespace Application.Models
{
    public class Error
    {
        public Error(long code, string message)
        {
            Code = code;
            Message = message;
        }
        public long Code { get; set; }
        public string Message { get; set; }
    }
}