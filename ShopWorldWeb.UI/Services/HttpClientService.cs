using Microsoft.Extensions.Configuration;
namespace ShopWorldWeb.UI.Services
{
    public class HttpClientService
    {
        private HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        public HttpClientService(IHttpContextAccessor httpContextAccessor) { 
            _httpContextAccessor = httpContextAccessor;
        }

        public HttpClient GetShopWorldClient() {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(ShopWorldGlobal.Url);

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
