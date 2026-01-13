using FeriaDelAgricultorController;
using FeriaDelAgricultorController.Abstractions;
using FeriaDelAgricultorModels;
using FeriaDelAgricultorWeb.Components;
using FeriaDelAgricultorWeb.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Sesión
builder.Services.AddScoped<SessionUsuarioService>();

// Controller/Handlers
builder.Services.AddScoped<UserHandler>();
builder.Services.AddScoped<LoginController>();

// IMPORTANTÍSIMO: interfaz y clase deben coincidir
builder.Services.AddScoped<IDataHandler<Usuario>, FileHandler>();

// Servicios existentes
builder.Services.AddScoped<PuntoFeriaService>();
builder.Services.AddScoped<ProductorService>();
builder.Services.AddScoped<ProductoService>();
builder.Services.AddScoped<CarritoService>();
builder.Services.AddScoped<FacturaService>();
builder.Services.AddScoped<EstadisticasService>();

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
