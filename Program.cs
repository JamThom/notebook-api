using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Notebook.Hubs;
using Notebook.Models;
using Notebook.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Notebook.Features;
using Microsoft.AspNetCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add SignalR
builder.Services.AddSignalR();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseSqlServer(connectionString);
});
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.User.RequireUniqueEmail = true;
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = false;
})
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.None;
    options.Events = new CookieAuthenticationEvents
    {
        OnRedirectToLogin = context =>
        {
            context.Response.StatusCode = 401;
            return Task.CompletedTask;
        },
        OnRedirectToAccessDenied = context =>
        {
            context.Response.StatusCode = 403;
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins",
        builder =>
        {
            builder.WithOrigins("http://localhost:3000", "https://witty-sky-05918a11e.6.azurestaticapps.net")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});

builder.Services.AddScoped<GetBooksFeature>();
builder.Services.AddScoped<GetBookFeature>();
builder.Services.AddScoped<GetPageFeature>();
builder.Services.AddScoped<CreatePageFeature>();
builder.Services.AddScoped<CreateNotebookFeature>();
builder.Services.AddScoped<LogoutFeature>();
builder.Services.AddScoped<RegisterFeature>();
builder.Services.AddScoped<LoginFeature>();
builder.Services.AddScoped<DeletePageFeature>();
builder.Services.AddScoped<DeleteNotebookFeature>();

var app = builder.Build();

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";
        var error = context.Features.Get<IExceptionHandlerFeature>();
        if (error != null)
        {
            var ex = error.Error;
            await context.Response.WriteAsync(ex.Message);
        }
    });
});

app.UseCors("AllowSpecificOrigins");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapHub<PageHub>("/pagehub");

app.Use(async (context, next) =>
{
    var allowedOrigins = new[] { "http://localhost:3000", "https://witty-sky-05918a11e.6.azurestaticapps.net" };
    var origin = context.Request.Headers["Origin"].ToString();
    if (allowedOrigins.Contains(origin))
    {
        context.Response.Headers["Access-Control-Allow-Origin"] = origin;
        context.Response.Headers["Access-Control-Allow-Methods"] = "POST, GET, OPTIONS, DELETE, PUT";
        context.Response.Headers.Append("Access-Control-Allow-Headers", "Content-Type, Authorization, X-Requested-With, X-SignalR-User-Agent");
        context.Response.Headers.Append("Access-Control-Allow-Credentials", "true");
        if (context.Request.Method == "OPTIONS")
        {
            context.Response.StatusCode = 204;
            return;
        }
    }
    else
    {
        context.Response.StatusCode = 403;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync($"{{\"message\": \"Origin is not allowed: {origin}\"}}");
        return;
    }
    await next();
});

app.MapControllers();

app.Run();