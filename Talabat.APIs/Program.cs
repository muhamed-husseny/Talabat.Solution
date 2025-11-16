
using Microsoft.EntityFrameworkCore;
using Talabat.APIs.Helpers;
using Talabat.Core.Entities.Products;
using Talabat.Core.Repositories.Contract;
using Talabat.Repository;
using Talabat.Repository.Data;

namespace Talabat.APIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Configure Services
            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            builder.Services.AddDbContext<StoreDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            }); 

            builder.Services.AddScoped(typeof(IGenaricRepositoriy<>), typeof(GenaricRepository<>));

            builder.Services.AddAutoMapper(typeof(MappingProfiles));

            #endregion

            var app = builder.Build();

            var scope = app.Services.CreateScope();

            var Services = scope.ServiceProvider;

            var _dbContext = Services.GetRequiredService<StoreDbContext>();
            // Ask CLR for Creating Object from dbContext Explicitly

            var LoggerFactory = Services.GetRequiredService<ILoggerFactory>();

            try
            {
                await _dbContext.Database.MigrateAsync(); // Update Database

                await StoreDbContextSeed.SeedAsync(_dbContext); // Data Seeding
            }
            catch (Exception ex)
            {
                var logger = LoggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "An error occurred during migration");

            }

            #region Configure Kestrel Middlewares
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();


            app.MapControllers(); 
            #endregion

            app.Run();
        }
    }
}
