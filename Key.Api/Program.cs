using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(), "Keys")))
    .SetDefaultKeyLifetime(TimeSpan.FromDays(10));

builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
    JwtBearerDefaults.AuthenticationScheme, opts =>
    {
        opts.Authority = "http://localhost:8080/realms/MyCompany";
        opts.RequireHttpsMetadata = false;
        opts.Audience = "Weather-Microservice";


        //opts.TokenValidationParameters = new TokenValidationParameters
        //{
        //    ValidateAudience = true,
        //    ValidateLifetime = true,
        //    ValidateIssuerSigningKey = true
        //};
    });

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
