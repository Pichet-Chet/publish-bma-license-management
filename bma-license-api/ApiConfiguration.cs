using bma_license_repository.CustomModel.Middleware;
using bma_license_service.Helper.Constants;

namespace bma_license_api
{
    public class ApiConfiguration
    {
        internal static object? GetResponseController(ResponseModel response)
        {
            if (response == null)
            {
                return new
                {
                    Status = ConstantsResponse.StatusError,
                    response.Code,
                    Message = "Response is null",
                };
            }

            if (response.Code == ConstantsResponse.HttpCode200)
            {
                if (response.Message == ConstantsResponse.HttpCode204Message)
                {
                    return new
                    {
                        response.Status,
                        response.Code,
                        response.Message,
                        response.Output,
                    };
                }

                if (response.RowCount == null)
                {
                    return new
                    {
                        response.Status,
                        response.Code,
                        response.Message,
                        response.Output,
                    };
                }

                return new
                {

                    response.Status,
                    response.Code,
                    response.Message,
                    response.PageNumber,
                    response.PageSize,
                    response.RowCount,
                    response.Output,

                };
            }



            // This handles both StatusError and any other status
            return new
            {
                response.Status,
                response.Code,
                response.Message
            };
        }
    }
}
