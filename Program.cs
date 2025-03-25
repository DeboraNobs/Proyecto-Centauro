using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using proyecto_centauro.Data;
using proyecto_centauro.Interfaces;
using proyecto_centauro.Repositorios;

var builder = WebApplication.CreateBuilder(args);

// configurar localización
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.AddControllers()
    .AddDataAnnotationsLocalization(options =>
    {
        options.DataAnnotationLocalizerProvider = (type, factory) =>
            factory.Create(typeof(proyecto_centauro.SharedResource));
    })
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
    });



// configurar la base de datos SQLite
builder.Services.AddDbContext<BBDDContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
builder.Services.AddScoped<IAlquilerRepositorio, AlquilerRepositorio>();
builder.Services.AddScoped<IServicioRepositorio, ServicioRepositorio>();
builder.Services.AddScoped<IGrupoRepositorio, GrupoRepositorio>();
builder.Services.AddScoped<ICocheRepositorio, CocheRepositorio>();

builder.Services.AddEndpointsApiExplorer();

// Configurar Swagger con idioma español
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "API de Proyecto Centauro",
        Version = "v1",
        Description = "Documentación de la API en español",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Soporte",
            Email = "soporte@proyecto-centauro.com"
        }
    });
});

var app = builder.Build();

// idioma español
var supportedCultures = new[] { new CultureInfo("es-ES") };
var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("es-ES"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
};

CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("es-ES");
CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("es-ES");

app.UseRequestLocalization(localizationOptions);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

