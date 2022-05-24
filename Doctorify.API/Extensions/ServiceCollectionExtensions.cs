using System.Reflection;
using System.Text;
using Doctorify.Infrastructure.Extensions;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;

namespace Doctorify.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddInfrastructure(builder.Configuration);
        builder.Services.AddJwtAuthentication(builder.Configuration);
        builder.Services.AddAuthorization();
        builder.Services.AddControllers();
        builder.Services.AddControllers()
               .AddNewtonsoftJson(s => s.SerializerSettings.ContractResolver =
                                           new CamelCasePropertyNamesContractResolver());
        builder.Services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly()));
        // builder.Services.AddApplication();
        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwagger();
    }

    // public static IServiceCollection AddApplication(this IServiceCollection services)
    // {
    //     var assembly = Assembly.GetExecutingAssembly();
    //     services.AddValidatorsFromAssembly(assembly);
    //
    //     return services;
    // }

    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        // create add swagger generator with security scheme and security requirements for JWT bearer token
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo {Title = "Doctorify API", Version = "v1"});
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                                              {
                                                  Description =
                                                      "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                                                  Name = "Authorization",
                                                  In = ParameterLocation.Header,
                                                  Type = SecuritySchemeType.ApiKey,
                                                  Scheme = "Bearer",
                                                  BearerFormat = "JWT"
                                              });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
                                     {
                                         {
                                             new OpenApiSecurityScheme
                                             {
                                                 Reference = new OpenApiReference
                                                             {
                                                                 Type = ReferenceType.SecurityScheme,
                                                                 Id = "Bearer"
                                                             }
                                             },
                                             Array.Empty<string>()
                                         }
                                     });
        });

        return services;
    }

    // create method AddJwtAuthentication to add JWT authentication with JWT Token and refresh token and JWT Token Validation and JWT Token Refresh and JWT Bearer Token Handler
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = new TokenValidationParameters()
                                                {
                                                    ValidateIssuer = true,
                                                    ValidateAudience = true,
                                                    ValidAudience = configuration["JWT:ValidAudience"],
                                                    ValidIssuer = configuration["JWT:ValidIssuer"],
                                                    IssuerSigningKey =
                                                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
                                                };
        });

        return services;
    }
}