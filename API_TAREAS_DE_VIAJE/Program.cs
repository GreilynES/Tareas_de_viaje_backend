using API_TAREAS_DE_VIAJE.Services.Database;
using API_TAREAS_DE_VIAJE.Services.Usuarios;
using API_TAREAS_DE_VIAJE.Services.Tareas;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Registrar servicio de Database
builder.Services.AddScoped<DatabaseService>();

// Registrar servicios
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<ITareaService, TareaService>();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

app.Run();