using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;
using MySqlConnector;
using travel_blog_api.Context;
using travel_blog_api.Repos;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseStaticFiles(); // wwwroot
// app.UseStaticFiles(new StaticFileOptions
// {
//     FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @".well-known")),
//     RequestPath = new PathString("/.well-known"),
//     ServeUnknownFileTypes = true // serve extensionless file
// });

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.UseAuthorization();

app.MapControllers();

#if DEBUG
app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
#endif

app.Run();
