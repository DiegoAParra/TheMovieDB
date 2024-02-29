using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TheMovieDB.Models;

namespace TheMovieDB.Services
{
    public class MovieApiService
    {
        private readonly HttpClient _httpClient;
        private const string ApiKey = "4bd2f81defd5257db82333d5c6759b81";
        private const string BaseUrl = "https://api.themoviedb.org/3";

        public MovieApiService()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(BaseUrl);
        }

        public async Task<List<Movie>> GetMoviesFromApiAsync()
        {
            var apiUrl = $"{BaseUrl}/movie/changes?api_key={ApiKey}";

            var response = await _httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var movieList = JsonConvert.DeserializeObject<MovieList>(jsonString);
                return movieList.Results;
            }
            else
            {
                throw new Exception($"Error al obtener la lista de películas. Código de estado: {response.StatusCode}");
            }
        }

        public async Task<Movie> GetMovieDetailsAsync(int movieId)
        {
            var apiUrl = $"{BaseUrl}/movie/{movieId}?api_key={ApiKey}";

            var response = await _httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var movieDetails = JsonConvert.DeserializeObject<Movie>(jsonString);
                return movieDetails;
            }
            else
            {
                throw new Exception($"Error al obtener los detalles de la película. Código de estado: {response.StatusCode}");
            }
        }

        
    }
}