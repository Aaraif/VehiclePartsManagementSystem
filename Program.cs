using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using VehiclePartsManagementSystem.Data;

var builder = WebApplication.CreateBuilder(args);

/* SERVICES */

builder.Services.AddControllersWithViews();

/* SESSION SUPPORT */

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout =
        TimeSpan.FromMinutes(30);

    options.Cookie.HttpOnly = true;

    options.Cookie.IsEssential = true;
});

/* API + SWAGGER */

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler =
            ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

/* DATABASE */

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString(
            "DefaultConnection")));

var app = builder.Build();

/* DEVELOPMENT */

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

    app.UseSwagger();

    app.UseSwaggerUI();
}

/* PRODUCTION */

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");

    app.UseHsts();
}

/* MIDDLEWARE */

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

/* SESSION */

app.UseSession();

app.UseAuthorization();

/* API ROUTES */

app.MapControllers();

/* MVC ROUTES */

app.MapControllerRoute(
    name: "default",
    pattern:
    "{controller=Account}/{action=Login}/{id?}");

app.Run();