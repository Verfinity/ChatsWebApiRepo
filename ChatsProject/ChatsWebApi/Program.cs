using ChatsWebApi.Components.Middlewares;
using ChatsWebApi.Components.Repositories.Chats;
using ChatsWebApi.Components.Repositories.Posts;
using ChatsWebApi.Components.Repositories.Users;
using ChatsWebApi.Components.Types.Database;
using ClassLibrary;
using ChatsWebApi.Components.Types.JWT.Options;
using ChatsWebApi.Components.Types.Roles;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ChatsWebApi.Components.Types.JWT.Logic.Creation;
using ChatsWebApi.Components.Repositories.ChatsToUsers;

namespace ChatsWebApi
{
    public class Program
    {
        public static async Task Main(string[] args)
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
                AddJwtBearer(jwtOptions =>
                {
                    jwtOptions.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidIssuer = authOptions.Issuer,

                        ValidateAudience = true,
                        ValidAudience = authOptions.Audience,

                        ValidateLifetime = true,

                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = authOptions.GetSymmetricSecurityKey()
                    };
                    jwtOptions.SaveToken = true;
                });
            builder.Services.AddAuthorization();
            builder.Services.AddSingleton<IAuthOptions>(authOptions);
            builder.Services.AddTransient<IJWTCreator, JWTCreator>();

            List<LoginFields> adminLogsList = builder.Configuration.GetSection("AdminLogs").Get<List<LoginFields>>();

            string? connStr = builder.Configuration.GetConnectionString("Postgres");
            builder.Services.AddDbContext<AppDBContext>(optionsBuilder =>
            {
                optionsBuilder.UseLazyLoadingProxies().UseNpgsql(connStr);
            });

            builder.Services.AddTransient<IUsersRepository, UsersRepository>();
            builder.Services.AddTransient<IChatsRepository, ChatsRepository>();
            builder.Services.AddTransient<IPostsRepository, PostsRepository>();
            builder.Services.AddTransient<IChatsUsersRepository, ChatsUsersRepository>();

            builder.Services.AddControllers();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDBContext>();
                await dbContext.Database.MigrateAsync();

                var usersRepo = scope.ServiceProvider.GetRequiredService<IUsersRepository>();
                foreach (var adminLog in adminLogsList)
                {
                    var admin = new User
                    {
                        NickName = adminLog.NickName,
                        Password = adminLog.Password,
                        Role = Role.Admin,
                        RefreshToken = Guid.NewGuid().ToString()
                    };
                    await usersRepo.CreateAsync(admin);
                }
            }

            //app.UseHttpsRedirection();

            app.UseMiddleware<ValidationExceptionMiddleware>();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
