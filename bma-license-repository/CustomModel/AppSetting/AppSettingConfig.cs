
namespace bma_license_repository.CustomModel.AppSetting
{
    public class AppSettingConfig
    {
        public class JWTConfig
        {
            public string Key { get; set; }
            public string TokenValidityInMinutes { get; set; }
            public string TokenValidityInDays { get; set; }
            public string RefreshTokenValidityInDays { get; set; }
            public string Issuer { get; set; }
            public string Audience { get; set; }
        }

        public class ConnectionStringsConfig
        {
            public string ConnectionStr { get; set; }
        }

    }
}
