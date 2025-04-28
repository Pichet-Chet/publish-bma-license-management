using bma_license_repository.Response.MessageAlert;

namespace bma_license_repository.Response.SysUser
{
    public class CreateUserResponse : ModelMessageAlertResponse
    {
        public CreateUserResponseData Data { get; set; }
    }

    public class CreateUserResponseData
    {
        public Guid Id { get; set; }
    }
}
