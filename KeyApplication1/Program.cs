using KeyApplication1.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpContextAccessor();

builder.Services.AddHttpClient<WeatherService>();
builder.Services.AddScoped<AutService>();

//data þifreleme 
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(),"Keys")))
    .SetDefaultKeyLifetime(TimeSpan.FromDays(10));

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
   
})
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, opt =>
    {
        opt.LoginPath = "/home/index";
        opt.ExpireTimeSpan = TimeSpan.FromDays(60);
        opt.SlidingExpiration = true;
        opt.Cookie.Name = "webCookie";
        opt.AccessDeniedPath = "/AcceessDenied";
      
    });
builder.Services.AddAuthorization(opt =>
{
    opt.AddPolicy("city", p =>
    {
        p.RequireClaim("city", "kayseri");
        
    });
});

//grand type
//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultScheme=CookieAuthenticationDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;

//}).AddCookie(opts => { opts.AccessDeniedPath = "/Home/AccessDenied"; }).AddOpenIdConnect(opt =>
//{
//    opt.RequireHttpsMetadata = false;
//    opt.Authority = "http://localhost:8080/realms/Thisrak";
//    opt.ClientId = builder.Configuration.GetSection("IdentityOption")["ClientId"];
//    opt.ClientSecret = builder.Configuration.GetSection("IdentityOption")["ClientSecret"];
//    opt.ResponseType = "code";//code isteniyor
//   // opt.GetClaimsFromUserInfoEndpoint = true;
//    opt.SaveTokens = true;//access token ve refresh tokeni cookie kaydetme
//    opt.Scope.Add("profile email address phone roles");//tüm bilgilendirme

//    opt.TokenValidationParameters = new TokenValidationParameters
//    {
//        NameClaimType = "preferred_username",
//        RoleClaimType="roles"

//    };


//    });




// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
