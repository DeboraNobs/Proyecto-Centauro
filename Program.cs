using Microsoft.EntityFrameworkCore;
using proyecto_centauro.Data;

var builder = WebApplication.CreateBuilder(args);

/* builder.Services.AddDbContext<UsuarioContext>(options =>
    options.UseSqlite("Data Source=usuarios.db")
); */ // nombre de la bbdd aquí
builder.Services.AddDbContext<UsuarioContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
