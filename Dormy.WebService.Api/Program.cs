using Dormy.WebService.Api.ApplicationLogic;
using Dormy.WebService.Api.Core.Interfaces;
using Dormy.WebService.Api.Infrastructure.Middlewares;
using Dormy.WebService.Api.Infrastructure.Postgres;
using Dormy.WebService.Api.Infrastructure.TokenRetriever;
using Dormy.WebService.Api.Startup;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

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
            builder.Services.AddSwaggerGen(opt =>
            {
                opt.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description = "Standard Authorization (\"bearer {token}\" ) ",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                });
                opt.OperationFilter<SecurityRequirementsOperationFilter>();
            });

            // Config JWT
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
                    };
                });

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

            // Add DI Repositories
            builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();

            // Add DI Services
            builder.Services.AddSingleton<ITokenRetriever, TokenRetriever>();
            builder.Services.AddScoped<IAdminService, AdminService>();
            builder.Services.AddScoped<IBedService, BedService>();
            builder.Services.AddScoped<IBuildingService, BuildingService>();
            builder.Services.AddScoped<IContractExtensionService, ContractExtensionService>();
            builder.Services.AddScoped<IContractService, ContractService>();
            builder.Services.AddScoped<IGuardianService, GuardianService>();
            builder.Services.AddScoped<IHealthInsuranceService, HealthInsuranceService>();
            builder.Services.AddScoped<IInvoiceItemService, InvoiceItemService>();
            builder.Services.AddScoped<IInvoiceService, InvoiceService>();
            builder.Services.AddScoped<INotificationService, NotificationService>();
            builder.Services.AddScoped<IOvernightAbsenceService, OvernightAbsenceService>();
            builder.Services.AddScoped<IParkingRequestService, ParkingRequestService>();
            builder.Services.AddScoped<IParkingSpotService, ParkingSpotService>();
            builder.Services.AddScoped<IRequestService, RequestService>();
            builder.Services.AddScoped<IRoomService, RoomService>();
            builder.Services.AddScoped<IRoomServiceService, RoomServiceService>();
            builder.Services.AddScoped<IRoomTypeService, RoomTypeService>();
            builder.Services.AddScoped<IRoomTypeServiceService, RoomTypeServiceService>();
            builder.Services.AddScoped<IServiceIndicatorService, ServiceIndicatorService>();
            builder.Services.AddScoped<ISettingService, SettingService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IVehicleHistoryService, VehicleHistoryService>();
            builder.Services.AddScoped<IVehicleService, VehicleService>();
            builder.Services.AddScoped<IViolationService, ViolationService>();
            builder.Services.AddScoped<IWorkplaceService, WorkplaceService>();
            builder.Services.AddScoped<IEmailService, EmailService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseRouting();

            app.UseAuthorization();
            
            app.MapControllers();

            // Middlewares
            app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

            // Migrate Db
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
                context.Database.Migrate();
            }

            app.Run();
        }
    }
}