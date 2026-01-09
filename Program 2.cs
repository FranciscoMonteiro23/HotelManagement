using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using HotelManagement.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

// Hotel Database
builder.Services.AddDbContext<HotelContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("HotelContextSQLite")));

// Identity Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("HotelManagementIdentityDb")));

// Identity
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 3;
})
.AddEntityFrameworkStores<ApplicationDbContext>();

// Configuração de Cookies do Identity
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    options.LoginPath = "/Identity/Account/Login";
    options.LogoutPath = "/Identity/Account/Logout";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.SlidingExpiration = true;
    options.Cookie.SameSite = SameSiteMode.Lax;
});

var app = builder.Build();

// Seed Data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    // Initialize Hotel Database
    var hotelContext = services.GetRequiredService<HotelContext>();
    DbInitializer.Initialize(hotelContext);

    // Initialize Identity Database
    var identityContext = services.GetRequiredService<ApplicationDbContext>();
    identityContext.Database.EnsureCreated();
}

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
    app.UseHttpsRedirection();
}
else
{
    // Em desenvolvimento, não use HTTPS redirection
    app.UseDeveloperExceptionPage();
}

app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();