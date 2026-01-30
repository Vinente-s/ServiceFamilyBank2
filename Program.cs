




using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using ServiceFamilyBank.Contexts;
using ServiceFamilyBank.Services;
using ServiceMonitoramentoWeb.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGen(options =>
{
    
    options.SwaggerDoc("v1",
    new OpenApiInfo
    {
        Version = "v1",
        Title = "Service Family Bank",
        Description = "API do Service Family Bank",
        Contact = new OpenApiContact
        {
            Name = "Gabriel Vinente",
            Email = "gabrielvinente144.gv@gmail.com"
        }
    }
    );
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpClient();

builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<HelperService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", corsBuilder =>
    {
        corsBuilder.WithOrigins("http://localhost:3000");
        corsBuilder.AllowAnyHeader();
        corsBuilder.AllowAnyMethod();
        corsBuilder.AllowCredentials();
    });
});

builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});

builder.Services.AddDbContext<AppDbContext>(option =>
{
    option.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
    option.EnableSensitiveDataLogging();
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseCors("CorsPolicy");
app.MapControllers();
app.Run();