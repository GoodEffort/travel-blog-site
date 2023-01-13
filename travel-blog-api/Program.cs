using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;
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
builder.Services.AddSingleton<IMongoDBRepo, MongoDBRepo>();

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
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @".well-known")),
    RequestPath = new PathString("/.well-known"),
    ServeUnknownFileTypes = true // serve extensionless file
});

app.UseAuthorization();

app.MapControllers();

#if DEBUG
app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
#endif

app.Run();
