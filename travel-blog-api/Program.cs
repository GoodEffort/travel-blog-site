using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using travel_blog_api.Context;
using travel_blog_api.Repos;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowSpecificOrigins",
    policy =>
    {
        policy.WithOrigins("http://localhost:5173", "https://api.lukestravelblog.com", "https://lukestravelblog.com", "https://www.lukestravelblog.com")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Travel Blog API",
        Description = "An ASP.NET Core Web API for getting photos from my google photos album."
    });
});

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new Exception("No connection string found");

builder.Services.AddDbContext<TravelblogContext>(options => options.UseMySQL(connectionString));
builder.Services.AddScoped<ITravelBlogRepo, TravelBlogDepo>();
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseStaticFiles(); // wwwroot

app.UseForwardedHeaders();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

#if DEBUG
app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
#endif

app.Run();
