using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;

namespace KeyApplication1.Services
{
    public class AutService(HttpClient client, ILogger<AutService> logger, IConfiguration configuration,IHttpContextAccessor httpContext)
    {
        public async Task<bool> SignIn(string email, string password)
        {

            var discoveryResponse =
                await client.GetDiscoveryDocumentAsync("http://localhost:8080/realms/Thisrak/.well-known/openid-configuration");
            if (discoveryResponse.IsError)
            {
                logger.LogError(discoveryResponse.Error);
            }

            var clientId = configuration.GetSection("IdentityOption")["ClientId"];
            var clientSecret = configuration.GetSection("IdentityOption")["ClientSecret"];

            var tokenResponse = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = discoveryResponse.TokenEndpoint,
                ClientSecret = clientSecret,
                ClientId = clientId,
                UserName = email,
                Password = password,
                Scope = "profile email address phone roles"
            });
            if (tokenResponse.IsError) { 
            logger.LogError(tokenResponse.Error);
            }

            var accessToken = tokenResponse.AccessToken;
            var handler = new JwtSecurityTokenHandler();
            
            var jsonToken=handler.ReadJwtToken(accessToken);

            var customClaimlist = new List<Claim>();
            var commenList = jsonToken.Claims.ToList();

            var realmAccessClaim = commenList.FirstOrDefault(c => c.Type == "realm_access");
            commenList.Remove(realmAccessClaim);
            if (realmAccessClaim is null) { 
            
            }
            var roleAsClaim = JsonSerializer.Deserialize<RoleAsClaim>(realmAccessClaim.Value);

          

            foreach (var role in roleAsClaim.Roles) { 
            customClaimlist.Add(new Claim(ClaimTypes.Role, role));
            }
            var userName = jsonToken.Claims.FirstOrDefault(c => c.Type == "preferred_username");
            commenList.Remove(userName);
            customClaimlist.Add(new Claim(ClaimTypes.Name,userName.Value));

            var userId = jsonToken.Claims.FirstOrDefault(c => c.Type == "sub");
            commenList.Remove(userId);
            customClaimlist.Add(new Claim(ClaimTypes.NameIdentifier,userId.Value));

            commenList.AddRange(customClaimlist);

            var identity=new ClaimsIdentity(commenList,"cookie",ClaimTypes.Name,ClaimTypes.Role);
            var principal=new ClaimsPrincipal(identity);

           await httpContext.HttpContext!.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,principal,null);



            return true;
        }


    }
}
