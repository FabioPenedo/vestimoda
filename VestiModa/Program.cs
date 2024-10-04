using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VestiModa.Context;
using VestiModa.Middleware;
using VestiModa.Models;
using VestiModa.Repositories;
using VestiModa.Repositories.Interfaces;
using VestiModa.Services;

var builder = WebApplication.CreateBuilder(args);

string connection = builder.Configuration.GetConnectionString("DefaultConnection")!;
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connection));

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

builder.Services.Configure<ConfigurationImages>(builder.Configuration.GetSection("ConfigurationPastaImagens"));

builder.Services.AddTransient<ICategoryRepository, CategoryRepository>();
builder.Services.AddTransient<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ISeedUserRoleInitial, SeedUserRoleInitial>();


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", politica =>
    {
        politica.RequireRole("Admin");
    });
});

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();


// Configura a sessão
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(10); // Tempo de expiração da sessão
    options.Cookie.HttpOnly = true; // Configura o cookie da sessão como HTTP only
    options.Cookie.IsEssential = true; // Torna o cookie essencial para a aplicação funcionar
});

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Habilita o middleware de sessão
app.UseSession();

await CriarPerfisUsuarios(app);

app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Admin}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "filterCategory",
    pattern: "Product/{action}/{category?}",
    defaults: new { controller = "Home", action = "Index" });


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
