using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using ObjetivoEventos.Application.Extensions;
using ObjetivoEventos.Identity.Constants;
using ObjetivoEventos.Identity.Extensions;
using ObjetivoEventos.Identity.Policies.HorarioComercial;
using Serilog.Context;
using System;
using System.Text;

namespace ObjetivoEventos.Application.Configuration
{
    public static class JwtConfig
    {
        public static void AddJwtTConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            IConfigurationSection jwtAppSettingOptions = configuration.GetSection(nameof(JwtOptions));
            SymmetricSecurityKey securityKey = new(Encoding.ASCII.GetBytes(configuration.GetSection("JwtOptions:SecurityKey").Value));

            services.Configure<JwtOptions>(options =>
            {
                options.Issuer = jwtAppSettingOptions[nameof(JwtOptions.Issuer)];
                options.Audience = jwtAppSettingOptions[nameof(JwtOptions.Audience)];
                options.SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512);
                options.AccessTokenExpiration = int.Parse(jwtAppSettingOptions[nameof(JwtOptions.AccessTokenExpiration)] ?? "0");
                options.RefreshTokenExpiration = int.Parse(jwtAppSettingOptions[nameof(JwtOptions.RefreshTokenExpiration)] ?? "0");
            });

            TokenValidationParameters tokenValidationParameters = new()
            {
                ValidateIssuer = false,
                ValidIssuer = configuration.GetSection("JwtOptions:Issuer").Value,

                ValidateAudience = true,
                ValidAudience = configuration.GetSection("JwtOptions:Audience").Value,

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = securityKey,

                RequireExpirationTime = true,
                ValidateLifetime = true,

                ClockSkew = TimeSpan.Zero
            };

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = tokenValidationParameters;
            });
        }

        public static void AddAuthorizationPolicies(this IServiceCollection services)
        {
            services.AddSingleton<IAuthorizationHandler, HorarioComercialHandler>();
            services.AddAuthorization(options =>
            {
                options.AddPolicy(Policies.HorarioComercial, policy =>
                    policy.Requirements.Add(new HorarioComercialRequirement()));
            });
        }

        public static void UseJwtConfiguration(this IApplicationBuilder app)
        {
            app.UseAuthentication();
            app.UseAuthorization();

            app.Use(async (httpContext, next) =>
            {
                string userName = httpContext.User.Identity.IsAuthenticated ? httpContext.User.GetUserEmail() : "Guest";
                LogContext.PushProperty("UserName", userName);
                await next.Invoke();
            });
        }
    }
}