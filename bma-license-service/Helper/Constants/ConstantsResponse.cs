namespace bma_license_service.Helper.Constants
{
    public class ConstantsResponse
    {
        public const bool StatusSuccess = true;
        public const bool StatusError = false;

        public const int HttpCode200 = 200;
        public const string HttpCode200Message = "OK";

        public const int HttpCode204 = 204;
        public const string HttpCode204Message = "No Content";

        public const int HttpCode300 = 300;
        public const string HttpCode300Message = "Multiple Choices";

        public const int HttpCode400 = 400;
        public const string HttpCode400Message = "Bad Request";

        public const int HttpCode401 = 401;
        public const string HttpCode401Message = "Unauthorized";

        public const int HttpCode402 = 402;
        public const string HttpCode402Message = "Payment Required Experimental";

        public const int HttpCode429 = 429;
        public const string HttpCode429Message = "Too many request";

        public const int HttpCode500 = 500;
        public const string HttpCode500Message = "Internal Server Error : The server has encountered a situation it does not know how to handle.";

        public const int HttpCode511 = 511;
        public const string HttpCode511Message = "Network Authentication Required.";
    }
}
