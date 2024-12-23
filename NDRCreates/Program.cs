using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using NDRCreates.Data;
using NDRCreates.Models.Entities;
using NDRCreates.Repositories;
using NDRCreates.Services.FileUploadService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddIdentity<BasicUser, BasicRole>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultUI()
    .AddDefaultTokenProviders()
    .AddUserManager<UserManager<BasicUser>>();

//builder.Services.AddDefaultIdentity<BasicUser>(options => options.SignIn.RequireConfirmedAccount = true)
//.AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();


builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddScoped<IFileUploadService, LocalFileStorageService>();
builder.Services.AddScoped<IUnityPackageRepository, UnityPackageRepository>();
builder.Services.AddScoped<IUnityPackageService, UnityPackageService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

// Handle User Role.
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<BasicRole>>();

    var roles = new[] { "Admin", "Manager", "Subscriber" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new BasicRole(role));
    }
}

// Create Admin Account.
using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<BasicUser>>();

    string email = "2200stellaris@gmail.com";
    string password = "2200stellaris@gmail.com";

    // No Admin? Create new one.
    if (await userManager.FindByEmailAsync(email) == null)
    {
        var user = new BasicUser();
        user.Email = email;
        user.UserName = "Nader Naderi";
        user.EmailConfirmed = true;

        await userManager.CreateAsync(user, password);

        await userManager.AddToRoleAsync(user, "Admin");
    }
}

app.Run();
