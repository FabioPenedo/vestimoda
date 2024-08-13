using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VestiModa.Context;
using VestiModa.Services;

var builder = WebApplication.CreateBuilder(args);

string connection = builder.Configuration.GetConnectionString("DefaultConnection")!;
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connection));

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();


builder.Services.AddScoped<ISeedUserRoleInitial, SeedUserRoleInitial>();


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", politica =>
    {
        politica.RequireRole("Admin");
    });
});

// Add services to the container.
builder.Services.AddControllersWithViews();

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

await CriarPerfisUsuarios(app);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

async Task CriarPerfisUsuarios(WebApplication app)
{
    var scoopedFactory = app.Services.GetService<IServiceScopeFactory>()!;
    using (var scope = scoopedFactory.CreateScope())
    {
        var service = scope.ServiceProvider.GetService<ISeedUserRoleInitial>()!;
        await service.CreateRoleAsync();
        await service.CreateUserAsync();
    }
}