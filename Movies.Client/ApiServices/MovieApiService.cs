using IdentityModel.Client;
using Movies.Client.Models;
using Newtonsoft.Json;

namespace Movies.Client.ApiServices
{
    public class MovieApiService : IMovieApiService
    {
        public async Task<IEnumerable<Movie>> GetMovies()
        {
            // 1. Get token from IS4

            // a) retrieve api credentials registered on IS4
            var apiClientCredentials = new ClientCredentialsTokenRequest
            {
                Address = "https://localhost:6005/connect/token",
                ClientId = "movieClient",
                ClientSecret = "secret",
                Scope = "movieAPI"
            };

            // - create a new HttpClient to talk to IS4
            var client = new HttpClient();

            // - checks if we can reach the Discovery document
            var discovery = await client.GetDiscoveryDocumentAsync("https://localhost:6005");
            if (discovery.IsError) return null; // throws 500 error

            // b) authenticates and gets access token from IS4
            var tokenResponse = await client.RequestClientCredentialsTokenAsync(apiClientCredentials);
            if (tokenResponse.IsError) return null;

            // 2. Send request to the protected API (which should include the token information)

            // - create another HttpClient to talk to the protected API
            var apiClient = new HttpClient();

            // a) set the bearer access_token in the request authoriation header
            apiClient.SetBearerToken(tokenResponse.AccessToken);

            // b) send request to the protected API
            var response = await apiClient.GetAsync("https://localhost:6001/api/movies");
            response.EnsureSuccessStatusCode();

            // 3. Deserialize object to Movie List

            var content = await response.Content.ReadAsStringAsync();
            List<Movie> movieList = JsonConvert.DeserializeObject<List<Movie>>(content);
            return movieList;

            //var movieList = new List<Movie>();
            //movieList.Add(new Movie
            //{
            //    Id = 1,
            //    Genre = "Comics",
            //    Title = "asd",
            //    Rating = "9.2",
            //    ImageUrl = "images/src",
            //    ReleaseDate = DateTime.Now,
            //    Owner = "swn"
            //});
            // return await Task.FromResult(movieList);
        }

        public Task<Movie> GetMovie(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Movie> CreateMovie(Movie movie)
        {
            throw new NotImplementedException();
        }

        public Task<Movie> UpdateMovie(Movie movie)
        {
            throw new NotImplementedException();
        }

        public Task DeleteMovie(int id)
        {
            throw new NotImplementedException();
        }
    }
}
