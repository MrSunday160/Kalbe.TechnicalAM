using Kalbe.TechnicalAM.DataAccess.Services;
using Kalbe.TechnicalAM.DataAccess.Services.Common;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using static Kalbe.TechnicalAM.DataAccess.Services.Common.JwtService;

namespace Kalbe.TechnicalAM.Api.Service {
    public static class ServiceRegistration {
        public static IServiceCollection AddRequiredServices(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment) {

            services.AddControllers().AddNewtonsoftJson(
                opt => {
                    opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                }
                );

            services.AddProblemDetails();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(opt => {

                opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });

                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type=ReferenceType.SecurityScheme,
                                    Id="Bearer"
                                }
                            },
                            Array.Empty<string>()
                        }
                    });

            }
                );

            services.ConfigureSwaggerGen(setup => {
                setup.SwaggerDoc("v1", new OpenApiInfo {
                    Title = "My API",
                    Version = "v1",
                    Contact = new OpenApiContact {
                        Name = "Justin Thejasukmana",
                        Email = "justinthejasukmana@gmail.com"
                    }
                });
            });

            services.AddHttpContextAccessor();

            #region Scopes

            //services.AddScoped<IBaseService, BaseService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<IJwtService, JwtService>();
            services.Configure<JwtConfiguration>(configuration.GetSection("Jwt"));
            services.AddScoped<ICartItemService, CartItemService>();

            services.AddScoped<ICheckoutService, CheckoutService>();

            #endregion

            return services;

        }

    }
}
