using System.Text.Json.Serialization;
using BlazorDemo.Server.Data;
using BlazorDemo.Server.Validators;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;

namespace BlazorDemo.Server;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        var connectionString = Configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddDatabaseDeveloperPageExceptionFilter();

        services.AddControllersWithViews()
            .AddFluentValidation(config =>
                config.RegisterValidatorsFromAssemblyContaining<TodoListValidator>())
            .AddJsonOptions(config =>
                config.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

        services.AddRazorPages();

        services.AddOpenApiDocument(configure =>
        {
            configure.Title = "BlazorDemo API";
        });
    }
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        // Configure the HTTP request pipeline.
        if (env.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
            app.UseWebAssemblyDebugging();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseBlazorFrameworkFiles();
        app.UseStaticFiles();

        app.UseSwaggerUi3(configure =>
            configure.DocumentPath = "/api/v1/specification.json");

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapRazorPages();
            endpoints.MapControllers();
            endpoints.MapFallbackToFile("index.html");
        });
    }
}
