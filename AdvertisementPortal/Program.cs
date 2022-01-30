using AdvertisementPortal;
using AdvertisementPortal.Authorization;
using AdvertisementPortal.Entities;
using AdvertisementPortal.Middleware;
using AdvertisementPortal.Models;
using AdvertisementPortal.Models.Validators;
using AdvertisementPortal.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog.Web;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder();
// NLog: Setup NLog for Dependency injection
builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
builder.Host.UseNLog();

// configure service
var authenticationSettings = new AuthenticationSettings();

builder.Configuration.GetSection("Authentication").Bind(authenticationSettings);

builder.Services.AddSingleton(authenticationSettings);
builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = "Bearer";
    option.DefaultScheme = "Bearer";
    option.DefaultChallengeScheme = "Bearer";
}).AddJwtBearer(cfg =>
{
    cfg.RequireHttpsMetadata = false;
    cfg.SaveToken = true;
    cfg.TokenValidationParameters = new TokenValidationParameters
{
        ValidIssuer = authenticationSettings.JwtIssuer,
        ValidAudience = authenticationSettings.JwtIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey)),
    };
});

builder.Services.AddScoped<IAuthorizationHandler, ResourceOperationRequirementHandler>();
builder.Services.AddControllers().AddFluentValidation();
builder.Services.AddDbContext<AdvertisementDbContext>(options => options
        .UseSqlServer(builder.Configuration.GetConnectionString("AdvertisementDbConnection")));
builder.Services.AddScoped<AdvertisementSeeder>();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddScoped<IAdvertisementService, AdvertisementService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ErrorHandlingMiddleware>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<IValidator<RegisterUserDto>, RegisterUserDtoValidator>();
builder.Services.AddScoped<IValidator<AdvertisementQuery>, AdvertisementQueryValidator>();
builder.Services.AddScoped<RequestTimeMiddleware>();
builder.Services.AddScoped<IUserContextService, UserContextService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "AdvertisementPortal", Version = "v1" });
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontEndClient", policyBuilder =>
        policyBuilder.AllowAnyMethod()
               .AllowAnyHeader()
               .WithOrigins(builder.Configuration["AllowedOrigins"])
        );
});
var app = builder.Build();
// configure

var scope = app.Services.CreateScope();
var seeder = scope.ServiceProvider.GetRequiredService<AdvertisementSeeder>();

app.UseResponseCaching();
app.UseStaticFiles();
app.UseCors("FrontEndClient");
seeder.Seed();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AdvertisementPortal v1"));
}


app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseMiddleware<RequestTimeMiddleware>();
app.UseAuthentication();
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();