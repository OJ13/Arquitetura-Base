using System.Collections.Generic;

namespace DDD.Helpers
{
    public class ApplicationSettings
    {
        public DatabaseSettings DatabaseSettings { get; set; }
        public Auth Auth { get; set; }
        public LogSettings LogSettings { get; set; }
        public OriginsCorsSettings OriginsCorsSettings { get; set; }
        public EmailSettings EmailSettings { get; set; }
    }
    public class DatabaseSettings
    {
        public string ConnectionStrings { get; set; }
    }
    public class Auth
    {
        public string JWTSecret { get; set; }
        public int Expiration { get; set; }
        public string ClientId { get; set; }
        public List<string> Scopes { get; set; }
        public int QtdSenhasVerificar { get; set; }
    }
    public class LogSettings
    {
        public string Log { get; set; }
        public string SeriLogPath { get; set; }
    }
    public class OriginsCorsSettings
    {
        public string[] UrlOrigins { get; set; }
    }
    public class EmailSettings
    {
        public string UrlServer { get; set; }
        public string PortServer { get; set; }
        public string EnableSsl { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string EmailFrom { get; set; }
    }
}
