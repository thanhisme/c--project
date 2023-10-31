using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;
using WebApplication1.Entities;
using WebApplication1.Interfaces.Repositories;
using WebApplication1.Interfaces.Services;
using WebApplication1.Services;
using WebApplication1.UnitOfWork;
using WebApplication1.Utils.HttpExceptionResponse;
using WebApplication1.Utils.UrlTransformer;

namespace WebApplication1.Infrastructure
{
    public static class ServiceRegistration
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ASMContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("SqlServer")
                )
            );

            services.AddScoped(_ => new BlobServiceClient(configuration.GetConnectionString("BlobStorage")));

            services
                .AddControllers(
                    options =>
                    {
                        options.Conventions.Add(new RouteTokenTransformerConvention(new UrlTransformer()));
                        options.Filters.Add<HttpExceptionResponseFilter>();
                        options.Filters.Add(new AuthorizeFilter());
                    }
                )
                .ConfigureApiBehaviorOptions(
                    options => options.SuppressModelStateInvalidFilter = true
                )
                .AddJsonOptions(
                    options => options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                );

            services.AddEndpointsApiExplorer().AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    In = ParameterLocation.Header,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    Description = "JWT Authorization header using the Bearer scheme."
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            services.AddAutoMapper(typeof(Program).Assembly);
            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateAudience = false,
                        ValidateIssuer = false,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(
                                configuration.GetSection("JwtSecretKey").Value!
                            )
                        )
                    };
                });

            services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();
            services.AddTransient<IUserService, UserService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IAuthService, AuthService>();

            return services;
        }
    }
}
