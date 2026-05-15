using CapitalSyndicate.Application.Crises;
using CapitalSyndicate.Application.Crises.Interfaces;
using CapitalSyndicate.Application.Partidas;
using CapitalSyndicate.Application.Turnos;
using CapitalSyndicate.Application.Turnos.Interfaces;
using CapitalSyndicate.Web.Components;
using CapitalSyndicate.Web.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped<EntradaJogadorBlazor>();
builder.Services.AddScoped<PartidaEstado>();
builder.Services.AddScoped<PartidaService>();
builder.Services.AddScoped<ITurnoService, TurnoService>();
builder.Services.AddScoped<ICriseService, CriseService>();

WebApplication app = builder.Build();

app.UsePathBase("/setup");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();