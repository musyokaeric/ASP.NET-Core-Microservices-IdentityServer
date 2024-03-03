using IdentityModel.Client;

namespace Movies.Client.HttpHandlers
{
    public class AuthenticationDelegatingHandler : DelegatingHandler
    {
        // Authentication delegation handler - retieves token from IS4
        // ===========================================================

        private readonly IHttpClientFactory httpClientFactory;
        private readonly ClientCredentialsTokenRequest tokenRequest;

        public AuthenticationDelegatingHandler(IHttpClientFactory httpClientFactory, ClientCredentialsTokenRequest tokenRequest)
        {
            this.httpClientFactory = httpClientFactory;
            this.tokenRequest = tokenRequest;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient("ISClient");

            var tokenResponse = await httpClient.RequestClientCredentialsTokenAsync(tokenRequest);
            if (tokenResponse.IsError) throw new HttpRequestException("Someting went wrong while requesting the access token");

            request.SetBearerToken(tokenResponse.AccessToken);

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
