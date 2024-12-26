using ChatsWebApi.Components.Repositories;
using ChatsWebApi.Components.Settings;
using ChatsWebApi.Components.Types;

namespace ChatsWebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            string? connStr = builder.Configuration.GetConnectionString("Default");
            builder.Services.AddSingleton(new DBSettings(connStr));

            builder.Services.AddTransient<IRepository<User>, UsersRepository>();

            builder.Services.AddControllers();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.MapControllers();

            app.Run();
        }
    }
}
