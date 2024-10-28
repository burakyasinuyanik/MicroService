using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//kimlik doðrulama
//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme=OpenIdConnectDefaults.AuthenticationScheme;
//}).AddCookie(opts => { opts.AccessDeniedPath = "/Home/AccessDenied"; })
//.AddOpenIdConnect(opts =>
//{
//    //keycloakconfigure
//    opts.RequireHttpsMetadata = false;
//    opts.Authority = "http://localhost:8080/realms/Thisrak";

//   });
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opt =>
{
    opt.Authority = "http://localhost:8080/realms/Thisrak";
    opt.RequireHttpsMetadata = false;
    opt.Audience = "weather-api";

    //opt.TokenValidationParameters=new TokenValidationParameters
    //{

    //};

    
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
