using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using ECommerce.SharedLibrary.DependecyInjection;
using ApiGateway.Presentation.Middleware;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
builder.Services.AddOcelot().AddCacheManager(x => x.WithDictionaryHandle());
JwtAuthenticationScheme.AddJwtAuthenticationScheme(builder.Services, builder.Configuration);
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseCors();
app.UseMiddleware<AttachSignatureToRequest>();
app.UseHttpsRedirection();
app.UseOcelot().Wait();
app.Run();

