using Microsoft.EntityFrameworkCore;
using SecurityIncidentTrackerWebApp.Data;
using Microsoft.AspNetCore.Identity;
using SecurityIncidentTrackerWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SecurityIncidentTrackerWebApp.Hubs;

var builder = WebApplication.CreateBuilder(args);

// CONFIGURARE BAZA DE DATE 
var connectionString = builder.Configuration.GetConnectionString("SecurityContext")
                      ?? builder.Configuration.GetConnectionString("SecurityContextConnection")
                      ?? throw new InvalidOperationException("Connection string not found.");

builder.Services.AddDbContext<SecurityContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddSignalR();

// CONFIGURARE IDENTITY
builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<SecurityContext>();

// Configurare pentru Mobil (API)
builder.Services.AddControllers().AddNewtonsoftJson(options => {
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    // Pastreaza numele proprietatilor exact ca in C# (PascalCase)
    options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver();
});

// Configurare pentru Site (Razor Pages)
builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeFolder("/");
    options.Conventions.AllowAnonymousToPage("/Index");
    options.Conventions.AllowAnonymousToPage("/Privacy");
}).AddNewtonsoftJson(); // Newtonsoft 

var app = builder.Build();

// CONFIGURARE MIDDLEWARE (Ordinea conteaza!)
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

// Comentat pentru a permite conexiunea HTTP de la emulator
// app.UseHttpsRedirection(); 

app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapRazorPages();

app.MapHub<NotificationHub>("/notificationHub");

app.Run();