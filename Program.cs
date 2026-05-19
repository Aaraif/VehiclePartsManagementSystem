using Microsoft.EntityFrameworkCore;
using VehiclePartsManagementSystem.Data;

var builder = WebApplication.CreateBuilder(args);

/* SERVICES */

builder.Services.AddControllersWithViews();

/* API + SWAGGER */

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

/* DATABASE */

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

/* GLOBAL EXCEPTION HANDLER */

app.UseExceptionHandler("/Home/Error");

/* DEVELOPMENT */

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI();
}

/* PRODUCTION */

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

/* MIDDLEWARE */

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

/* API ROUTES */

app.MapControllers();

/* MVC ROUTES */

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();