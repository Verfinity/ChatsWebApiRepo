using ChatsWebApi.Components.Repositories.Chats;
using ChatsWebApi.Components.Repositories.Posts;
using ChatsWebApi.Components.Repositories.Users;
using ChatsWebApi.Components.Types.Database;
using ChatsWebApi.Components.Types.JWT;
using ChatsWebApi.Components.Types.Roles;
using ChatsWebApi.Components.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ChatsWebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
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

            string? connStr = builder.Configuration.GetConnectionString("Default");
            builder.Services.AddDbContext<AppDBContext>(optionsBuilder =>
            {
                optionsBuilder.UseLazyLoadingProxies().UseSqlServer(connStr);
            });

            builder.Services.AddTransient<IValidator<User>, UserValidator>();
            builder.Services.AddTransient<IValidator<Post>, PostValidator>();
            builder.Services.AddTransient<IValidator<Chat>, ChatValidator>();
            builder.Services.AddTransient<IValidator<LoginFields>, LoginFieldsValidator>();

            builder.Services.AddTransient<IUsersRepository, UsersRepository>();
            builder.Services.AddTransient<IChatsRepository, ChatsRepository>();
            builder.Services.AddTransient<IPostsRepository, PostsRepository>();

            builder.Services.AddControllers();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
