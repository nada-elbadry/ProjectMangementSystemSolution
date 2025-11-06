using GymMangementBLL;
using GymMangementBLL.Services.Classes;
using GymMangementBLL.Services.Interfaces;
using GymMangementDAL.Data.Contexts;
using GymMangementDAL.Data.DataSeed;
using GymMangementDAL.Repositories.Classes;
using GymMangementDAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace ProjectMangementPL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<GymDbContext>(options =>
            {

                //        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found."));
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            //builder.Services.AddScoped<ITrainerRepository,TrainerRepository>();
            //builder.Services.AddScoped<ISessionRepository, SessionRepository>();
            //builder.Services.AddScoped(typrof(<IGenaricRepository<>),typeof(GenaricRepository<>));
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<ISessionRepository, SessionRepository>();
            builder.Services.AddAutoMapper(X=>X.AddProfile(new MappingProfile()));
            builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();
            builder.Services.AddScoped<IMemberService, MemberService>();
            var app = builder.Build();

            #region   Migrate Database Data Seeding

            using var Scoped = app.Services.CreateScope();
            var dbContext = Scoped.ServiceProvider.GetRequiredService<GymDbContext>();
            var PendingMigartions = dbContext.Database.GetPendingMigrations();
            //if (PendingMigartions?.Any() ?? false)
            //    dbContext.Database.Migrate();
          //  GymDbContextSeeding.SeedData(dbContext);

            #endregion

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();

            //app.MapControllerRoute(
            //    name: "Trainers",
            //    pattern: "Coach/{action}",
            //    defaults: new { controller = "Trainer" , action ="Index"}
            //    );
            //BaseUrl/Controller/Action/Id
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
