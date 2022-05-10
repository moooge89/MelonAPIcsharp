namespace MelonAPI.Model.exception
{
    public class RestException : Exception
    {
        public RestException(string message) : base(message)
        {
        }
    }
}
