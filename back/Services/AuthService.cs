using System.Text;

namespace MyFirstApi.Services
{
    public interface IAuthService
    {
        string GenerateToken(string username);
        string? GetUsernameFromToken(string token);
        Task<string?> ValidateTokenFromRequest(HttpRequest request);
    }

    public class AuthService : IAuthService
    {
        private const string SECRET_KEY = "ffiptXLrFAGMspu8M5s2Snh6ZOWqMe54";

        public string GenerateToken(string username)
        {
            string tokenData = $"{username}:{DateTime.UtcNow.Ticks}:{SECRET_KEY}";
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(tokenData));
        }

        public string? GetUsernameFromToken(string token)
        {
            try
            {
                byte[] tokenBytes = Convert.FromBase64String(token);
                string tokenData = Encoding.UTF8.GetString(tokenBytes);
                string[] parts = tokenData.Split(':');
                
                if (parts.Length != 3) return null;

                var timestamp = long.Parse(parts[1]);
                if (new DateTime(timestamp) < DateTime.UtcNow.AddHours(-1))
                    return null;

                return parts[0];
            }
            catch
            {
                return null;
            }
        }

        public async Task<string?> ValidateTokenFromRequest(HttpRequest request)
        {
            var token = request.Cookies["authToken"];
            if (string.IsNullOrEmpty(token))
                return null;

            return GetUsernameFromToken(token);
        }
    }
}