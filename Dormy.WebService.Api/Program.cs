using Dormy.WebService.Api.Infrastructure.Postgres;
using Dormy.WebService.Api.Startup;
using Microsoft.EntityFrameworkCore;

namespace Dormy.WebService.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<ApplicationContext>(options =>
            {
                options.UseNpgsql(builder.Configuration.GetConnectionString("LocalConnection"));
            });

            // Fix postgres datetime
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            // Add CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });

            builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseAuthorization();

            app.UseRouting();
            
            app.MapControllers();

            using (var scope = app.Services.CreateScope())
            {
                //var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
                //context.Database.Migrate();
            }

            app.Run();
        }
    }
}