using Microsoft.AspNetCore.Authentication.Cookies;
using ShopWorldWeb.UI.Models.Profiles;
using ShopWorldWeb.UI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();
builder.Services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<HttpClientService>();
builder.Services.AddScoped<ShopWorldClient>();
builder.Services.AddAutoMapper(typeof(CustomerProfiler));
builder.Services.AddAutoMapper(typeof(ItemProfiler));
builder.Services.AddAutoMapper(typeof(OrderProfiler));
builder.Services.AddAutoMapper(typeof(EmployeeProfile));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseAuthentication();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
