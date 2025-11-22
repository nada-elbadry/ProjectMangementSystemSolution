using GymManagementBLL.Services.Attachment_Service;
using GymMangementBLL;
using GymMangementBLL.Services.Attachment_Service;
using GymMangementBLL.Services.Classes;
using GymMangementBLL.Services.Interfaces;
using GymMangementDAL.Data.Contexts;
using GymMangementDAL.Data.DataSeed;
using GymMangementDAL.Entities;
using GymMangementDAL.Repositories.Classes;
using GymMangementDAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
namespace ProjectMangementPL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            #region  Builder Configuration

            var builder = WebApplication.CreateBuilder(args);

            #endregion

            #region  Services Registration
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
            builder.Services.AddScoped<ITrainerServices, TrainerService>();
            builder.Services.AddScoped<IPlanService, PlanService>();
            builder.Services.AddScoped<ISessionService, SessionService>();
            builder.Services.AddScoped<IAttachmentService, AttachmentService>();
            #region Login
            builder.Services.AddScoped<IAccountService, AccountService>();
            #endregion

            builder.Services.AddIdentity < ApplicationUser ,IdentityRole>(Config=>
            {
                //Defult:
                //Config.Password.RequiredLength = 6;
                //Config.Password.RequireLowercase = true;
                //Config.Password.RequireUppercase = true;
                Config.User.RequireUniqueEmail = true;

            })
                            .AddEntityFrameworkStores<GymDbContext>();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LogoutPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/AccessDenied";
            });


            #endregion

            #region  App Build

            var app = builder.Build();

            #endregion

            #region   Migrate Database Data Seeding

            using var Scoped = app.Services.CreateScope();
            var dbContext = Scoped.ServiceProvider.GetRequiredService<GymDbContext>();
            var roleManager = Scoped.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManger = Scoped.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var PendingMigartions = dbContext.Database.GetPendingMigrations();
            if (PendingMigartions?.Any() ?? false)
                dbContext.Database.Migrate();
            GymDbContextSeeding.SeedData(dbContext);
            IdentityDbContextSeeding.SeedData(roleManager, userManger);

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
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();
            #region  Endpoint Mapping
            //app.MapControllerRoute(
            //    name: "Trainers",
            //    pattern: "Coach/{action}",
            //    defaults: new { controller = "Trainer" , action ="Index"}
            //    );
            //BaseUrl/Controller/Action/Id
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Login}/{id?}")
                .WithStaticAssets();
#endregion

            #region  Run Application
            app.UseStaticFiles();
            app.Run();
            #endregion
        }
    }
}
