using Lab11_API.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Agregar soporte para que lea tu ReportesController
builder.Services.AddControllers();

// 2. Configurar la conexión a MySQL con tu usuario y contraseña
string connectionString = "Server=localhost;Port=3306;Database=LINQExample;Uid=root;Pwd=9707;";

// Conectamos usando el archivo exacto que Rider generó: LinqexampleContext
builder.Services.AddDbContext<LinqexampleContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

var app = builder.Build();

// 3. Configuraciones de rutas
app.UseHttpsRedirection();
app.MapControllers();

// 4. Iniciar la aplicación
app.Run();