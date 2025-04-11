using ChatsWebApi.Components.Middlewares;
using ChatsWebApi.Components.Repositories.Chats;
using ChatsWebApi.Components.Repositories.Posts;
using ChatsWebApi.Components.Repositories.Users;
using ChatsWebApi.Components.Types.Database;
using ChatsWebApi.Components.Types.JWT;
using ChatsWebApi.Components.Types.Roles;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography.X509Certificates;

namespace ChatsWebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

            var authConfigs = builder.Configuration.GetSection("Authentication");
            IAuthOptions authOptions = new AuthOptions
            {
                Issuer = authConfigs["Issuer"],
                Audience = authConfigs["Audience"],
                Key = authConfigs["Key"]
            };
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).
                AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidIssuer = authOptions.Issuer,

                        ValidateAudience = true,
                        ValidAudience = authOptions.Audience,

                        ValidateLifetime = true,

                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = authOptions.GetSymmetricSecurityKey()
                    };
                });
            builder.Services.AddAuthorization();
            builder.Services.AddSingleton<IAuthOptions>(authOptions);

            List<LoginFields> adminLogsList = builder.Configuration.GetSection("AdminLogs").Get<List<LoginFields>>();
            IAdminLogs adminLogs = new AdminLogs
            {
                AdminLogsList = adminLogsList.ToArray()
            };
            builder.Services.AddSingleton<IAdminLogs>(adminLogs);

            string? connStr = builder.Configuration.GetConnectionString("Postgres");
            builder.Services.AddDbContext<AppDBContext>(optionsBuilder =>
            {
                optionsBuilder.UseLazyLoadingProxies().UseNpgsql(connStr);
            });

            builder.Services.AddTransient<IUsersRepository, UsersRepository>();
            builder.Services.AddTransient<IChatsRepository, ChatsRepository>();
            builder.Services.AddTransient<IPostsRepository, PostsRepository>();

            builder.Services.AddControllers();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseHttpsRedirection();

            app.UseMiddleware<ValidationExceptionMiddleware>();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
