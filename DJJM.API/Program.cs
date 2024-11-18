using DJJM.API.Data;
using DJJM.API.Interfaces;
using DJJM.API.Models;
using DJJM.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace DJJM.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Bind and configure JwtSettings
            var jwtSettingsSection = builder.Configuration.GetSection("JwtSettings");
            builder.Services.Configure<JwtSettings>(jwtSettingsSection);

            // Register the DbContext with PostgreSQL provider
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
            );

            // Add Identity services
            builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                // Password settings
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            // Add Email Sender Service
            builder.Services.AddTransient<IEmailSender, EmailSender>();

            // Register and validate JwtSettings
            builder.Services.AddSingleton<IValidateOptions<JwtSettings>, JwtSettingsValidation>();

            var app = builder.Build();

            // Validate JwtSettings after building the service provider
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var options = services.GetRequiredService<IOptions<JwtSettings>>().Value;

                if (string.IsNullOrEmpty(options.Secret) ||
                    string.IsNullOrEmpty(options.Issuer) ||
                    string.IsNullOrEmpty(options.Audience))
                {
                    throw new InvalidOperationException("JWT Settings are not properly configured in appsettings.json.");
                }
            }

            // Configure JWT authentication
            var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
            if (string.IsNullOrEmpty(jwtSettings?.Secret))
            {
                throw new InvalidOperationException("JWT Settings are not properly configured in appsettings.json.");
            }
            var key = Encoding.UTF8.GetBytes(jwtSettings!.Secret);

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
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });

            // Add controllers
            builder.Services.AddControllers();

            // Register Swagger services
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var builtApp = builder.Build();

            // Configure the HTTP request pipeline.
            if (builtApp.Environment.IsDevelopment())
            {
                builtApp.UseSwagger();
                builtApp.UseSwaggerUI();
            }

            builtApp.UseAuthentication(); // Ensure authentication middleware is added before authorization
            builtApp.UseAuthorization();

            builtApp.MapControllers();

            builtApp.Run();
        }
    }
    
    // Custom validation class
    public class JwtSettingsValidation : IValidateOptions<JwtSettings>
    {
        public ValidateOptionsResult Validate(string? name, JwtSettings options)
        {
            // Ensure options is not null (defensive programming)
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options), "JWT settings cannot be null.");
            }

            // Validate required fields
            if (string.IsNullOrEmpty(options.Secret))
            {
                return ValidateOptionsResult.Fail("JWT Secret is not configured.");
            }

            if (string.IsNullOrEmpty(options.Issuer))
            {
                return ValidateOptionsResult.Fail("JWT Issuer is not configured.");
            }

            if (string.IsNullOrEmpty(options.Audience))
            {
                return ValidateOptionsResult.Fail("JWT Audience is not configured.");
            }

            // Return success if all validations pass
            return ValidateOptionsResult.Success;
        }
    }
}
