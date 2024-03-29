using System.Text.Json.Serialization;
using Boxtorio.Common;
using Boxtorio.Configs;
using Boxtorio.Data;
using Boxtorio.Middlewares;
using Boxtorio.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Boxtorio;

internal sealed class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var authSection = builder.Configuration.GetSection(AuthConfig.Position);
        var authConfig = authSection.Get<AuthConfig>();

        builder.Services.Configure<AuthConfig>(authSection);

        builder.Services.AddControllers();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme,
                new OpenApiSecurityScheme
                {
                    Description = "Введите токен авторизации",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = JwtBearerDefaults.AuthenticationScheme
                });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = JwtBearerDefaults.AuthenticationScheme
                        },
                        Scheme = "oauth2",
                        Name = JwtBearerDefaults.AuthenticationScheme,
                        In = ParameterLocation.Header
                    },
                    new List<string>()
                }
            });
        });

        builder.Services.AddAutoMapper(typeof(MapperProile).Assembly);
        builder.Services.AddScoped<AccountService>();
        builder.Services.AddScoped<AuthService>();
        builder.Services.AddScoped<DeliveryPointService>();
        builder.Services.AddScoped<BoxService>();

        builder.Services.AddAuthentication(o =>
            {
                o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = false;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = authConfig?.Issuer,
                    ValidateAudience = true,
                    ValidAudience = authConfig?.Audience,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = authConfig?.SymmetricSecurityKey(),
                    ClockSkew = TimeSpan.Zero
                };
            });

        builder.Services.AddAuthorization(o =>
        {
            o.AddPolicy("ValidAccessToken", p =>
            {
                p.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
                p.RequireAuthenticatedUser();
            });
        });

        builder.Services.AddDbContext<DataContext>(options =>
        {
            options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSql"), _ => { });
        });

        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                options.JsonSerializerOptions.WriteIndented = true;
            });

        var app = builder.Build();

        using (var serviceScope = ((IApplicationBuilder)app).ApplicationServices.GetService<IServiceScopeFactory>()?.CreateScope())
        {
            if (serviceScope != null)
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<DataContext>();
                context.Database.Migrate();
            }
        }

        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();
        app.UseTokenValidator();
        app.MapControllers();

        app.Run();
    }
}
