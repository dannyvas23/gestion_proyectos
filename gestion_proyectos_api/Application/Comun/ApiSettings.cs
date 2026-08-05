
namespace Application.Comun
{
    public class ApiSettings
    {

        public string SaltGeneradorHash { get; set; } = "";

        #region JWT_CONFIG
        public string Key { get; set; } = "";
        public string Issuer { get; set; } = "";
        public string Audience { get; set; } = "";

        #endregion

    }
}
