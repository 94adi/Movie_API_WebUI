using System.Text;

namespace Movie.API
{
    public static class Utilities
    {
        public static byte[] GetPrivateKey(this WebApplicationBuilder builder)
        {
            var appSettingsSection = builder.Configuration.GetSection("AppSettings");
            builder.Services.Configure<AppSettings>(appSettingsSection);

            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

            return key;
        }
    }
}
