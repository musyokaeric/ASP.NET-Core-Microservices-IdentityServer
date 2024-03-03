using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace Movies.Client.HttpHandlers
{
    public class AuthenticationDelegatingHandler : DelegatingHandler
    {
        // Authentication delegation handler - retieves token from IS4
        // ===========================================================

        // ********************************************************************************
        // Not needed since we only need HttpContextAccessor to access this infomration
        // ********************************************************************************

        //private readonly IHttpClientFactory httpClientFactory;
        //private readonly ClientCredentialsTokenRequest tokenRequest;

        //public AuthenticationDelegatingHandler(IHttpClientFactory httpClientFactory, ClientCredentialsTokenRequest tokenRequest)
        //{
        //    this.httpClientFactory = httpClientFactory;
        //    this.tokenRequest = tokenRequest;
        //}

        private readonly IHttpContextAccessor httpContextAccessor;

        public AuthenticationDelegatingHandler(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            //var httpClient = httpClientFactory.CreateClient("ISClient");

            //var tokenResponse = await httpClient.RequestClientCredentialsTokenAsync(tokenRequest);
            //if (tokenResponse.IsError) throw new HttpRequestException("Someting went wrong while requesting the access token");

            //request.SetBearerToken(tokenResponse.AccessToken);

            var accessToken = await httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
            if (!string.IsNullOrEmpty(accessToken)) request.SetBearerToken(accessToken);

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
