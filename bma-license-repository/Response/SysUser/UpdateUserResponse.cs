
using bma_license_repository.Response.MessageAlert;

namespace bma_license_repository.Response.SysUser
{
    public class UpdateUserResponse : ModelMessageAlertResponse
    {
        public UpdateUserResponseData Data { get; set; }

    }

    public class UpdateUserResponseData
    {
        public Guid Id { get; set; }
    }
}
