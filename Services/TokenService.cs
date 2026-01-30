using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using ServiceFamilyBank.Dtos.Usuario;



namespace ServiceFamilyBank.Services
{
    public class TokenService
    {

        private readonly IConfiguration _configuration;
        public enum ClaimType
        {
            JwtToken,
            email, 
            codigo,
            nome,
            perfil
        }

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void CreateCookies(HttpContext httpContext, Dictionary<string, (string, bool)> tokens)
        {
            foreach (var token in tokens)
            {
                string key = token.Key;
                string value = token.Value.Item1;
                bool httpOnly = token.Value.Item2;

                httpContext.Response.Cookies.Append(key, value, new CookieOptions
                {
                    HttpOnly = httpOnly,
                    Secure = true,
                    IsEssential = true,
                    SameSite = SameSiteMode.None,
                    Expires = DateTime.Now.AddHours(2),
                    Domain = _configuration["Jwt:Host"],
                });
            }
        }

        public string GenerateToken(LoggedUsuarioDto user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>{
             new Claim("codigo", user.codigo.ToString()),
             new Claim("email", user.email),
             new Claim("perfil", user.fk_perfil.ToString()),
             new Claim("nome", user.nome),
            };

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string? GetTokenFromCookie(HttpContext httpContext)
        {
            ArgumentNullException.ThrowIfNull(httpContext);

            return httpContext.Request.Cookies["JwtToken"];
        }

        public string GetTokenValue(string token, ClaimType key)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken
                ?? throw new Exception("Invalid token");

            var claim = jsonToken.Claims.FirstOrDefault(claim => claim.Type == key.ToString()) ?? throw new Exception("Claim not found");
            return claim.Value;
        }

    }

}