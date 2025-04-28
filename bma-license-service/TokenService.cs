

using bma_license_service.Helper;
using static bma_license_repository.CustomModel.AppSetting.AppSettingConfig;
using static bma_license_repository.Helper.ConstantsMessage;

namespace bma_license_service
{
    public interface ITokenService
    {
        Task<string> GetToken();
    }
    public class TokenService : ITokenService
    {
        private readonly JWTConfig _jwtConfig;
        private readonly DateTime _datetime;

        public TokenService(JWTConfig jwtConfig)
        {
            _jwtConfig = jwtConfig;
            _datetime = DateTime.UtcNow;
        }

        public async Task<string> GetToken()
        {
            System.DateTime expierDate = await HelperService.GetTokenExpiredDefault(_jwtConfig, _datetime, false);
            string token = HelperService.GenerateToken(_jwtConfig.Key, _jwtConfig.Issuer, _jwtConfig.Audience, expierDate, HelperService.GenerateGuid(), RoleConfig.RoleAdmin);
            return $"Bearer {token}";
        }
    }
}
