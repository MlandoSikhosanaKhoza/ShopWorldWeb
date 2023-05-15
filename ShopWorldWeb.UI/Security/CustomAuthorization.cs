using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.IdentityModel.Tokens.Jwt;
using ShopWorld.Shared;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace ShopWorldWeb.UI.Security
{
    /* To be implemented (Inprogress with experiment)*/
    public class CustomAuthorization : Attribute, IAuthorizationFilter
    {
        public string Roles { get; set; }

        /// <summary>  
        /// This will Authorize User  
        /// </summary>  
        /// <returns></returns>  
        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            bool isValid = false;
            if (filterContext != null)
            {
                if (filterContext.HttpContext != null)
                {
                    if(filterContext.HttpContext.Request != null)
                    {
                        if(filterContext.HttpContext.Request.Cookies != null)
                        {
                            IRequestCookieCollection cookies = filterContext.HttpContext.Request.Cookies;
                            if (cookies.ContainsKey("login_token"))
                            {
                                
                                JwtSecurityToken jwtSecurityToken = JwtTokenReader.GetJwtToken(cookies["login_token"]);
                                isValid = IsValidUser(cookies["login_token"]);
                                if (isValid)
                                {
                                    List<Claim> claims = new List<Claim>();
                                    claims.AddRange(jwtSecurityToken.Claims);
                                    claims.Add(new Claim("login_token", cookies["login_token"]));
                                    filterContext.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(claims));
                                }
                                
                            }
                        }
                    }
                }
                var obj = filterContext.HttpContext.GetEndpoint().Metadata.GetMetadata<IAllowAnonymous>();
                if (!isValid && obj==null)
                {
                    filterContext.Result = new RedirectResult("/Home/Index");
                }
                
            }
        }

        public bool IsValidUser(string Token)
        {
            if (string.IsNullOrEmpty(Roles))
            {
                return true;
            }
            JwtSecurityToken jwtSecurityToken = JwtTokenReader.GetJwtToken(Token);
            string[] roles=(string.IsNullOrEmpty(Roles))?new string[0]:Roles.Split(',');
            foreach (var role in roles)
            {
                if (roles.Any(r => r == role))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
