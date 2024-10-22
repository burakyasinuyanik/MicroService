
using Docker.API.Models;

namespace Docker.API.Service
{
    public class StockService(HttpClient client)
    {
        public  async Task<int> GetStock() {
            var response = await client.GetAsync("/api/Stock/GetStock");
            if (response.IsSuccessStatusCode) { 
            var content=await response.Content.ReadFromJsonAsync<GetStockResponse>();
                return content.count;
            }
            return 0; 
        }
    }
}
