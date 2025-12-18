using FeriaDelAgricultorController;
using FeriaDelAgricultorWeb.Components;
using Microsoft.AspNetCore.Components;

var builder = WebApplication.CreateBuilder(args);

// Blazor (Server / Interactive)
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// ✅ Registrar tus servicios (Controller)
// Usamos Singleton para que la app mantenga carrito/servicios vivos durante la sesión.
// (Si tu profe exige otro, luego lo cambiamos, pero esto funciona bien.)
builder.Services.AddSingleton<ProductoService>();
builder.Services.AddSingleton<ProductorService>();
builder.Services.AddSingleton<PuntoFeriaService>();
builder.Services.AddSingleton<CarritoService>();
builder.Services.AddSingleton<FacturaService>();

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
