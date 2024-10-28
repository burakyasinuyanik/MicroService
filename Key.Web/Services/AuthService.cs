using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
using IdentityModel.Client;

namespace Key.Web.Services
{
    public class AuthService(
        HttpClient client,
        ILogger<AuthService> logger,
        IConfiguration configuration,
        IHttpContextAccessor httpContextAccessor)
    {

        public async Task<bool> SignIn(string email, string password)
        {
            var discoveryResponse =
                await client.GetDiscoveryDocumentAsync(
                    "http://localhost:8080/realms/Thisrak/.well-known/openid-configuration");

            if (discoveryResponse.IsError)
            {
                logger.LogError(discoveryResponse.Error);
            }

            var clientId = configuration.GetSection("IdentityOption")["ClientId"]!;
            var clientSecret = configuration.GetSection("IdentityOption")["ClientSecret"]!;
            var tokenResponse = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = discoveryResponse.TokenEndpoint,
                ClientId = clientId,
                ClientSecret = clientSecret,
                UserName = email,
                Password = password,
                Scope = "profile email address phone roles"
            });

            if (tokenResponse.IsError)
            {
                logger.LogError(discoveryResponse.Error);
            }


            var accessToken = tokenResponse.AccessToken;

            var handler = new JwtSecurityTokenHandler();

            var jsonToken = handler.ReadJwtToken(accessToken);
            var customClaimList = new List<Claim>();
            var commonClaimList = jsonToken.Claims.ToList();

            var realmAccessClaim = commonClaimList.FirstOrDefault(c => c.Type == "realm_access");
            commonClaimList.Remove(realmAccessClaim);


            var roleAsClaim = JsonSerializer.Deserialize<RoleAsClaim>(realmAccessClaim.Value);


            foreach (var role in roleAsClaim.Roles)
            {
                customClaimList.Add(new Claim(ClaimTypes.Role, role));
            }

            var userNameAsClaim = commonClaimList.FirstOrDefault(c => c.Type == "preferred_username");

            commonClaimList.Remove(userNameAsClaim);

            customClaimList.Add(new Claim(ClaimTypes.Name, userNameAsClaim.Value));


            var subAsClaim = jsonToken.Claims.FirstOrDefault(c => c.Type == "sub");

            commonClaimList.Remove(subAsClaim);


            customClaimList.Add(new Claim(ClaimTypes.NameIdentifier, subAsClaim.Value));


            commonClaimList.AddRange(customClaimList);


            var identity = new ClaimsIdentity(commonClaimList, "cookie", ClaimTypes.Name, ClaimTypes.Role);

            var principal = new ClaimsPrincipal(identity);


            await httpContextAccessor.HttpContext!.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                null);

            return true;
        }
    }
}
