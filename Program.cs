using back_project.Areas.Admin.Services;
using back_project.DAL;
using back_project.Data;
using Microsoft.EntityFrameworkCore;

namespace back_project
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();


            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddScoped<CategoryService>();
            builder.Services.AddScoped<SpeakerService>();

            Constants.RootPath = builder.Environment.WebRootPath;
            Constants.SliderPath = Path.Combine(Constants.RootPath, "assets", "img", "slider");
            Constants.TestimonialSliderPath = Path.Combine(Constants.RootPath, "assets", "img", "testimonial");
            Constants.TeacherPath = Path.Combine(Constants.RootPath, "assets", "img", "teacher");
            Constants.CoursePath = Path.Combine(Constants.RootPath, "assets", "img", "course");
            Constants.BlogPath = Path.Combine(Constants.RootPath, "assets", "img", "blog");
            Constants.SpeakerAndEventPath = Path.Combine(Constants.RootPath, "assets", "img", "event");

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                  name: "areas",
                  pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"
                );

                endpoints.MapControllerRoute(
                  name: "default",
                  pattern: "{controller=Home}/{action=Index}/{id?}"
                );
            });

            app.Run();
        }
    }
}