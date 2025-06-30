using Proyecto.DAL.DataAccess;

var builder = WebApplication.CreateBuilder(args);

// 🔹 Leer cadena de conexión desde appsettings.json
var connectionString = builder.Configuration.GetConnectionString("MiConexion")
    ?? throw new InvalidOperationException("La cadena de conexión 'MiConexion' no está configurada.");


// 🔹 Registrar la fábrica de conexiones y clases DAL
builder.Services.AddSingleton(new DbConnectionFactory(connectionString));
builder.Services.AddScoped<RegionDAL>();
builder.Services.AddScoped<ComunaDAL>();

// 🔹 Servicios base de la API
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 🔹 Middleware y configuración del entorno
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
