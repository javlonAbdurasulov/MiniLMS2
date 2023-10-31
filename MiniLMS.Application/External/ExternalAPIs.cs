using Microsoft.Extensions.Configuration;
using MiniLMS.Domain.Entities;
using MiniLMS.Domain.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace MiniLMS.Application.External
{
    public class ExternalAPIs : IExternalAPIs
    {

        private readonly HttpClient _httpClientServicesCat;
        private readonly HttpClient _httpClientServicesValspeak;
        private readonly HttpClient _httpClientServicesWeather;
        private readonly Uri _uriGet;
        private readonly Uri _uriCreate;
        private readonly Uri _uriUpdate;
        private readonly Uri _uriDelete;

        public ExternalAPIs(IHttpClientFactory httpClient, IConfiguration configuration)
        {
            _httpClientServicesCat = httpClient.CreateClient();
            _httpClientServicesCat.BaseAddress = new Uri(configuration.GetSection("ExternalAPI")["Cats"]);

            _httpClientServicesValspeak = httpClient.CreateClient();
            _httpClientServicesValspeak.BaseAddress = new Uri(configuration.GetSection("ExternalAPI")["Valspeak"]);

            _httpClientServicesWeather= httpClient.CreateClient();
            //_httpClientServicesWeather.BaseAddress = new Uri($"http://localhost:5289");
            _uriGet = new Uri($"{configuration.GetSection("ExternalAPI")["WeatherGet"]}");
            _uriCreate = new Uri($"{configuration.GetSection("ExternalAPI")["WeatherCreate"]}");
            _uriUpdate = new Uri($"{configuration.GetSection("ExternalAPI")["WeatherUpdate"]}");
            _uriDelete = new Uri($"{configuration.GetSection("ExternalAPI")["WeatherDelete"]}");
            _httpClientServicesWeather.BaseAddress = _uriGet;
            //_httpClientServicesWeather.BaseAddress = new Uri($"{configuration.GetSection("ExternalAPI")["Weather"]}/api/WeatherForecast");
        }
        public async Task<ResponseModel<Catfact>> GetAllCatFacts()
        {

            HttpResponseMessage catFacts = await _httpClientServicesCat.
                GetAsync($"{_httpClientServicesCat.BaseAddress}");
            Catfact? facts = await catFacts.Content.ReadFromJsonAsync<Catfact>();

            return new(facts);
        }
        public async Task<ResponseModel<List<Weather>>> GetWeather()
        {
            _httpClientServicesWeather.BaseAddress = _uriGet;
            //_httpClientServicesWeather.BaseAddress = new Uri($"{_httpClientServicesWeather.BaseAddress}/api/WeatherForecast/Get");
            HttpResponseMessage response = await _httpClientServicesWeather.GetAsync("");
            //HttpResponseMessage response = await _httpClientServicesWeather.GetAsync($"{_httpClientServicesWeather.BaseAddress}/api/WeatherForecast/Get");
            List<Weather> resoinseModel = await response.Content.ReadFromJsonAsync<List<Weather>>();
            
            return new(resoinseModel);
        }
        
        public async Task<ResponseModel<Weather>> CreateWeather(Weather weather)
        {
            _httpClientServicesWeather.BaseAddress = _uriCreate;

            HttpResponseMessage response = await _httpClientServicesWeather.
                PostAsJsonAsync("", weather);
            Weather? responseModel = await response.Content.ReadFromJsonAsync<Weather>();

            return new(responseModel);
        }
        public async Task<ResponseModel<Weather>> updateWeather(Weather weather)
        {
            _httpClientServicesWeather.BaseAddress = _uriUpdate;
            //object models = new { id, weather };
            var response = await _httpClientServicesWeather.PutAsJsonAsync("", weather);
            return new(weather);
        }
        
        public async Task<ResponseModel<bool>> deleteWeather(int id)
        {
            string s = _uriDelete.ToString();
            _httpClientServicesWeather.BaseAddress = new Uri($"{s}?id={id}");
            //StringContent stringContent = new StringContent(id.ToString(), Encoding.UTF8, "text/plain");
            var response = await _httpClientServicesWeather.DeleteAsync("");
            return new(true);
        }



    }
}
