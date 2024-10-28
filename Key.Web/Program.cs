using Key.Web.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.DataProtection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient<WeatherService>();
builder.Services.AddScoped<AuthService>();
//Resource Owner Grant Types
//builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(
//    CookieAuthenticationDefaults.AuthenticationScheme, opts =>
//    {
//        opts.LoginPath = "/Auth/SignIn";
//        opts.ExpireTimeSpan = TimeSpan.FromDays(60);
//        opts.SlidingExpiration = true;
//        opts.Cookie.Name = "webCookie";
//        opts.AccessDeniedPath = "/AccessDenied";
//    });
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(), "Keys")))
    .SetDefaultKeyLifetime(TimeSpan.FromDays(10));

//builder.Services.AddAuthorization(x => { x.AddPolicy("city", y => { y.RequireClaim("city", "kayseri"); }); });

////Authorization Code Grant Type Example
builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
    }).AddCookie(opts => { opts.AccessDeniedPath = "/Home/AccessDenied"; })
    .AddOpenIdConnect(opts =>
    {
        opts.RequireHttpsMetadata = false;

        opts.Authority = "http://localhost:8080/realms/Thisrak";
        opts.ClientId = builder.Configuration.GetSection("IdentityOption")["ClientId"]!;
        opts.ClientSecret = builder.Configuration.GetSection("IdentityOption")["ClientSecret"];
        opts.ResponseType = "code";

        opts.GetClaimsFromUserInfoEndpoint = true;
        opts.SaveTokens = true;
        opts.Scope.Add("profile email address phone roles");


        //opts.TokenValidationParameters = new TokenValidationParameters
        //{
        //    NameClaimType = "preferred_username",
        //    RoleClaimType = "roles"
        //};


        //opts.Events.OnRedirectToIdentityProvider += context =>
        //{
        //    context.Options.Authority = "http://localhost:8080/realms/ExampleTenant";

        //    return Task.CompletedTask;
        //};
    });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();

app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
