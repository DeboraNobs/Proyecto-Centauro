using Microsoft.EntityFrameworkCore;
using proyecto_centauro.Data;
using proyecto_centauro.Interfaces;
using proyecto_centauro.Repositorios;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text.Json.Serialization;
using System.Reflection;
using proyecto_centauro.RepositoriosDapper;
using proyecto_centauro.RepositoriosRequest;
using proyecto_centauro.Interfaces.InterfacesValidation;
using proyecto_centauro.Business;
using proyecto_centauro.Interfaces.InterfacesBusiness;
using System.Data;
using System.Data.SQLite;


var builder = WebApplication.CreateBuilder(args);

// habilitar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000") 
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

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
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
    });


// configurar la base de datos SQLite
builder.Services.AddDbContext<BBDDContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IDbConnection>(provider =>
    new SQLiteConnection(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// repositorios 
builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorioDapper>(); // antes era UsuarioRepositorio
builder.Services.AddScoped<IAlquilerRepositorio, AlquilerRepositorio>();
builder.Services.AddScoped<IServicioRepositorio, ServicioRepositorio>();
builder.Services.AddScoped<IGrupoRepositorio, GrupoRepositorio>();
builder.Services.AddScoped<ICocheRepositorio, CocheRepositorioDapper>(); // antes CocheRepositorio
builder.Services.AddScoped<ISucursalRepositorioV, SucursalRepositorioRequest>(); // antes era SucursalRepositorio, y SucursalRepositorioDapper y he cambiado el repositorio (era sin V)
// builder.Services.AddScoped<ISucursalRepositorio, SucursalRepositorio>();

// business
builder.Services.AddScoped<ISucursalBusiness, SucursalBusiness>();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddLogging(options =>
{
    options.AddConsole(); 
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "tuApp",
        ValidAudience = "tuApp",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]!)), // busca en appsettings.json la key definida en JWT
        ValidateIssuer = false,
        ValidateAudience = false,
    };
});

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

var app = builder.Build();

app.UseCors("AllowReactApp");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

