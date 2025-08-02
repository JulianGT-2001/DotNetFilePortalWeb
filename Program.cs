
using System.Text;
using DotNetEnv;
using fileuploadweb.Models.Dto;
using fileuploadweb.Negocio.Contrato;
using fileuploadweb.Negocio.Logica;
using fileuploadweb.Services.Contrato;
using fileuploadweb.Services.Logica;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.IdentityModel.Tokens;

Env.Load();

var baseGatewayUrl = Environment.GetEnvironmentVariable("BASE_GATEWAY_URL");
var authGatewayUrl = Environment.GetEnvironmentVariable("AUTH_GATEWAY_URL");
var fileGatewayUrl = Environment.GetEnvironmentVariable("FILE_GATEWAY_URL");
var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY");
var jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER");
var jwtAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE");
var jwtDuration = Environment.GetEnvironmentVariable("JWT_DURATION");

var builder = WebApplication.CreateBuilder(args);

builder.Configuration["GatewayUrls:Auth"] = baseGatewayUrl + authGatewayUrl;
builder.Configuration["GatewayUrls:Files"] = baseGatewayUrl + fileGatewayUrl;
builder.Configuration["Jwt:Key"] = jwtKey;
builder.Configuration["Jwt:Issuer"] = jwtIssuer;
builder.Configuration["Jwt:Audience"] = jwtAudience;
builder.Configuration["Jwt:Duration"] = jwtDuration;

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    };

    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var token = context.Request.Cookies["jwt_token"];
            Console.WriteLine("Token recibido: " + token);
            if (!string.IsNullOrEmpty(token))
            {
                context.Token = token;
            }

            return Task.CompletedTask;
        }
    };
});

builder.Services.AddScoped<IAuth, AuthLogica>();
builder.Services.AddScoped<IFile, FileLogica>();
builder.Services.AddScoped<IHttp, HttpLogica>();
builder.Services.AddScoped<HttpClient>();
builder.Services.Configure<GatewayUrls>(builder.Configuration.GetSection("GatewayUrls"));

builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo("/root/.aspnet/DataProtection-Keys"));

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");

app.Run();
