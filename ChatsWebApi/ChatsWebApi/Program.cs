using ChatsWebApi.Components.Repositories.Chats;
using ChatsWebApi.Components.Repositories.Posts;
using ChatsWebApi.Components.Repositories.Users;
using ChatsWebApi.Components.Settings;
using ChatsWebApi.Components.Types.Database;
using ChatsWebApi.Components.Types.JWT;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
            AuthOptions authOptions = new AuthOptions
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


            string? connStr = builder.Configuration.GetConnectionString("Default");
            builder.Services.AddSingleton(new DBSettings(connStr));
            builder.Services.AddSingleton(authOptions);

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
