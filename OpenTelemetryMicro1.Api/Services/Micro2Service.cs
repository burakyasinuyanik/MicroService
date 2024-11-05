using System.Text.Json.Serialization;

namespace OpenTelemetryMicro1.Api.Services
{
    public class Micro2Service(HttpClient client)
    {
        public async Task<string> GetMicro2Data(){

            var response = await client.PostAsJsonAsync<Product>("https://localhost:7241/api/products", new Product(10,"test",23));

            return await response.Content.ReadAsStringAsync();


        }
    }
}
