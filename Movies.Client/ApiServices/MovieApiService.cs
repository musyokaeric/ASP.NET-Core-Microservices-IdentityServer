using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Movies.Client.Models;
using Newtonsoft.Json;

namespace Movies.Client.ApiServices
{
    public class MovieApiService : IMovieApiService
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IHttpContextAccessor httpContextAccessor;
        private HttpClient httpClient;

        public MovieApiService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            this.httpClientFactory = httpClientFactory;
            this.httpContextAccessor = httpContextAccessor;
            httpClient = httpClientFactory.CreateClient("MovieAPIClient");
        }

        public async Task<UserInfoViewModel> GetUserInfo()
        {
            var client = httpClientFactory.CreateClient("ISClient");

            var metadataResponse = await client.GetDiscoveryDocumentAsync();
            if (metadataResponse.IsError) throw new HttpRequestException("Something went wrong while requesting the access token");

            var accessToken = await httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);

            var userInfoResponse = await client.GetUserInfoAsync(
                new UserInfoRequest
                {
                    Address = metadataResponse.UserInfoEndpoint,
                    Token = accessToken
                });
            if (userInfoResponse.IsError) throw new HttpRequestException("Something went wrong while getting user info");

            var userInfoDictionary = new Dictionary<string, string>();
            foreach (var claim in userInfoResponse.Claims)
            {
                userInfoDictionary.Add(claim.Type, claim.Value);
            }

            return new UserInfoViewModel(userInfoDictionary);
        }

        public async Task<IEnumerable<Movie>> GetMovies()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/movies/");

            var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            List<Movie> movieList = JsonConvert.DeserializeObject<List<Movie>>(content);
            return movieList;
        }

        public async Task<Movie> GetMovie(string id)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"/api/movies/{id}");

            var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            Movie movie = JsonConvert.DeserializeObject<Movie>(content);
            return movie;
        }

        public async Task<Movie> CreateMovie(Movie movie)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/movies/")
            {
                Content = JsonContent.Create(movie)
            };

            var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Movie>(content);
        }

        public async Task<Movie> UpdateMovie(Movie movie)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, $"/api/movies/{movie.Id}")
            {
                Content = JsonContent.Create(movie)
            };

            var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Movie>(content);
        }

        public async Task DeleteMovie(int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, $"/api/movies/{id}");

            var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
        }
    }
}
