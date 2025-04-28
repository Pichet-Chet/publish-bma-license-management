
using System.ComponentModel.DataAnnotations;

namespace bma_license_repository.Request.SysUser
{
    public class VerifyUserRequest
    {
        [Required(ErrorMessage = "UserName is required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
