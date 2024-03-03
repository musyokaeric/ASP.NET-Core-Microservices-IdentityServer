using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Security.Claims;

namespace IdentityServer
{
    public class Config
    {
        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                // **************************************************************************************
                // Not needed since the scope and be accessed within the MVC web app's client scope below
                // **************************************************************************************

                //new Client // for the API web application
                //{
                //    ClientId = "movieClient",
                //    AllowedGrantTypes = GrantTypes.ClientCredentials,
                //    ClientSecrets = { new Secret("secret".Sha256()) },
                //    AllowedScopes = { "movieAPI" }
                //},
                new Client // for the MVC web application
                {
                    ClientId = "movies_mvc_client",
                    ClientName = "Movies MVC Web App",
                    AllowedGrantTypes = GrantTypes.Hybrid, // hybrid authentication flow
                    RequirePkce = false,
                    AllowRememberConsent = false,
                    RedirectUris = new List<string>
                    {
                        "https://localhost:6002/signin-oidc" // this is the client app port
                    },
                    PostLogoutRedirectUris = new List<string>
                    {
                        "https://localhost:6002/signout-callback-oidc"
                    },
                    ClientSecrets = new List<Secret>
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "movieAPI" // added the API scope
                    }
                }
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("movieAPI","Movie API")
            };

        public static IEnumerable<ApiResource> ApiResources =>
            new ApiResource[] { };

        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[] 
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };

        public static List<TestUser> TestUsers =>
            new List<TestUser> 
            {
                new TestUser
                {
                    SubjectId = "5BE86359-073C-434B-AD2D-A3932222DABE",
                    Username = "eric",
                    Password = "Password01!",
                    Claims = new List<Claim>
                    {
                        new Claim(JwtClaimTypes.GivenName, "eric"),
                        new Claim(JwtClaimTypes.FamilyName, "musyoka")
                    }
                }
            };
    }
}
