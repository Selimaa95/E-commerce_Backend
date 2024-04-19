
namespace API.Errors
{
    public class ApiResponse
    {
        public int statusCode {  get; set; }
        public string message { get; set; }
        public ApiResponse(int statusCode, string message = null)
        {
            this.statusCode = statusCode;
            this.message = message ?? GetDefaultMessageForStatusCode(statusCode);
        }

        private string GetDefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "A Bad Request, you have made",
                401 => "Authorized you are not",
                404 => "Request found it is not",
                500 => "Server Error Occured",
                _ => null
            };
        }
    }
}
