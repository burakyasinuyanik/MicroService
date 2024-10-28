using System.Text.Json.Serialization;

namespace Key.Web.Services
{
    public class RoleAsClaim
    {
        [JsonPropertyName("roles")]
        public List<string>? Roles { get; set; }
    }
}
