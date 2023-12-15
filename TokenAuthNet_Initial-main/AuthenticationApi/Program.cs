using AuthenticationApi.Helpers;
using Google.Authenticator;
using TwoFactor;

using Microsoft.Extensions.Configuration;

using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("LaunchSettings"));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();
//services.AddAuthentication()
//    .AddGoogleAuthenticator("GoogleAuthenticator", options =>
//    {
//        options.Secret = "YourSecretKey";
//        options.QrCodeUri = "YourQRCodeUri";
//    });



//builder.Services.AddAuthentication(options =>
//     {
//         options.DefaultAuthenticateScheme = Google.;
//         options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
//     })
//            .AddGoogle(options =>
//            {
//                options.ClientId = "[MyGoogleClientId]";
//                options.ClientSecret = "[MyGoogleSecretKey]";
//            });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

