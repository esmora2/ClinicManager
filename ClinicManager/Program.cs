using ClinicManager.Services;
using ClinicManager;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve);
// **Configuraci�n de servicios**
// Agregar controladores


// Configurar Swagger para la documentaci�n de la API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configurar la conexi�n a la base de datos
builder.Services.AddDbContext<AppDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registrar servicios personalizados
builder.Services.AddScoped<CitasService>();
builder.Services.AddScoped<DoctoresService>();
builder.Services.AddScoped<PacientesService>();
builder.Services.AddScoped<ProcedimientosService>();

var app = builder.Build();

// **Configuraci�n del pipeline**
if (app.Environment.IsDevelopment())
{
    // Activar Swagger solo en el entorno de desarrollo
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Redirecci�n HTTPS
app.UseHttpsRedirection();

// Middleware de autenticaci�n y autorizaci�n (si se usa en el futuro)
app.UseAuthorization();

// Mapear controladores
app.MapControllers();

// Iniciar la aplicaci�n
await app.RunAsync();
