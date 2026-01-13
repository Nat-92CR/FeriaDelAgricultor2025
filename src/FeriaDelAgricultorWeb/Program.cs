using FeriaDelAgricultorController;
using FeriaDelAgricultorController.Abstractions;
using FeriaDelAgricultorModels;
using FeriaDelAgricultorWeb.Components;
using FeriaDelAgricultorWeb.Services;

var builder = WebApplication.CreateBuilder(args);

// =========================
// Blazor Server / Interactive
// =========================
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Sesión de usuario (por conexión)
builder.Services.AddScoped<SessionUsuarioService>();

// =========================
// Inyección de dependencias (Controller / Services)
// =========================

// Usuarios / Login
builder.Services.AddScoped<IDataHandler<Usuario>, FileHandler>();
builder.Services.AddScoped<UserHandler>();
builder.Services.AddScoped<LoginController>();

// Catálogos (se cargan desde CSV y se mantienen en memoria)
builder.Services.AddSingleton<ProductorService>();
builder.Services.AddSingleton<ProductoService>();
builder.Services.AddSingleton<PuntoFeriaService>();

// Flujo de compra
builder.Services.AddScoped<CarritoService>();
builder.Services.AddScoped<FacturaService>();

// Reportes / Estadísticas
builder.Services.AddSingleton<EstadisticasService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
   .AddInteractiveServerRenderMode();

app.Run();
