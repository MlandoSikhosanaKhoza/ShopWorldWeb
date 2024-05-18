using Microsoft.Extensions.Configuration;
using System.Security.Claims;

namespace ShopWorldWeb.UI.Services
{
    public class HttpClientService
    {
        private HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        public HttpClientService(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration       = configuration;
        }

        public HttpClient GetShopWorldClient() {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(_configuration.GetValue<string>("Settings:ApiUrl"));
            Claim login_token = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "login_token");


            if (login_token!=null)
            {
                string jwt =login_token.Value;
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwt);
            }

            return _httpClient;
        }
    }
}
