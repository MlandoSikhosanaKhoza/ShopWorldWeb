using Microsoft.Extensions.Configuration;
namespace ShopWorldWeb.UI.Services
{
    public class HttpClientService
    {
        private HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        public HttpClientService(IHttpContextAccessor httpContextAccessor,IConfiguration configuration) { 
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        public HttpClient GetShopWorldClient() {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(_configuration["API:ShopWorld"]);

            IRequestCookieCollection cookies = _httpContextAccessor.HttpContext.Request.Cookies;
            
            if (cookies!=null && cookies.ContainsKey("login_token"))
            {
                string jwt = cookies["login_token"];
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bear", jwt);
            }

            return _httpClient;
        }
    }
}
