using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ProjetoIntegrador.Data;
using ProjetoIntegrador.Interfaces;
using ProjetoIntegrador.Services;
using ProjetoIntegrador;
using System.Text;
using Hangfire;
using Hangfire.SQLite; // Se você usa SQLite, certifique-se de que está configurado corretamente
using Hangfire.MemoryStorage; // ou UseInMemoryStorage

class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var key = Encoding.UTF8.GetBytes(Settings.Secret);
        builder.Services.AddControllers();
        builder.Services.AddControllersWithViews();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "API Projeto Integrador", Version = "v1" });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "Autorização de JWT está definida com o esquema Bearer",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer"
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
                    new string[] { }
                }
            });
            c.OperationFilter<SwaggerFileOperationFilter>();
        });

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAllOrigins",
                builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
        });

        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite(builder.Configuration.GetConnectionString("Default")));

        builder.Services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(x =>
        {
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = "http://192.168.0.99:5203",
                ValidateAudience = true,
                ValidAudience = "http://192.168.0.99:5203",
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
        });

        builder.Services.AddTransient<IHashServices, HashServices>();
        builder.Services.AddTransient<ITokenServices, TokenServices>();
        builder.Services.AddTransient<IImageServices, ImageServices>();

        builder.Services.AddScoped<EmailServices>();

        // Usando Hangfire MemoryStorage, se essa for a intenção.
        // Se quiser usar SQLite, certifique-se de ter a dependência correta e a string de conexão.
        builder.Services.AddHangfire(configuration =>
            configuration.UseSimpleAssemblyNameTypeSerializer()
                         .UseRecommendedSerializerSettings()
                         .UseMemoryStorage());
        // Se fosse SQLite, seria algo como:
        // configuration.UseSQLiteStorage(builder.Configuration.GetConnectionString("HangfireConnection"));

        builder.Services.AddHangfireServer();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Projeto Integrador v1");
            });
        }

        app.UseCors("AllowAllOrigins");
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseHangfireDashboard("/hangfire");

        app.Use(async (context, next) =>
        {
            if (context.Request.Path == "/")
            {
                context.Response.Redirect("/login");
                return;
            }
            await next();
        });

        app.MapControllers();
        app.UseStaticFiles();

        app.Run();
    }
}