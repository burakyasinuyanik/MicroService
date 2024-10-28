using System.Text.Json.Serialization;

namespace KeyApplication1.Services
{
    public class RoleAsClaim
    {
        [JsonPropertyName("roles")]
        public List<string>? Roles { get; set; }
    }
}
