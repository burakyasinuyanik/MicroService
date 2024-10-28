﻿using IdentityModel.Client;

namespace KeyApplication1.Services
{
    public class WeatherService(HttpClient client,ILogger<WeatherService> logger,IConfiguration configuration)
    {
        public async Task<List<WeatherForecastResponseDto>> GetWeather()
        {

           var discoveryResponse= await client.GetDiscoveryDocumentAsync("http://localhost:8080/realms/Thisrak/.well-known/openid-configuration");

            if (discoveryResponse.IsError) { 
           logger.LogError(discoveryResponse.Error);
            }
            var clientId = configuration.GetSection("IdentityOption")["ClientId"]!;
            var clientSecret = configuration.GetSection("IdentityOption")["ClientSecret"]!;


            var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = discoveryResponse.TokenEndpoint,
                ClientId =clientId, 
                ClientSecret =clientSecret
            });

            if (tokenResponse.IsError) {
                logger.LogError(discoveryResponse.Error);
            }
            client.SetBearerToken(tokenResponse.AccessToken!);


           var response= await client.GetAsync("http://localhost:5189/WeatherForecast");
            if (response.IsSuccessStatusCode) {
                logger.LogError(response.ReasonPhrase);

            }

            var weatherList=await response.Content.ReadFromJsonAsync<List<WeatherForecastResponseDto>>();

            return weatherList!;
        }


            
    }
}
