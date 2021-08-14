using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using KSE.WebAppMvc.Extensions.Middleware;

namespace KSE.WebAppMvc.Configuration
{
    public static class WebAppConfig
    {
        public static void AddWebAppConfig(this IServiceCollection services)
        {
            services.AddRazorPages().AddRazorRuntimeCompilation();
        }
        public static void AppWebAppConfig(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}
            //else
            //{
            //    app.UseExceptionHandler("/erro/500");
            //    app.UseStatusCodePagesWithRedirects("/erro/{0}");
            //    app.UseHsts();
            //}

            app.UseExceptionHandler("/erro/500");
            app.UseStatusCodePagesWithRedirects("/erro/{0}");
            app.UseHsts();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.AppIdentityConfig();

            app.UseMiddleware<ExceptionMiddleware>();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
