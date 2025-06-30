var builder = WebApplication.CreateBuilder(args);

// ?? Agrega el HttpClient apuntando a tu API REST
builder.Services.AddHttpClient("ApiClient", client =>
{
    client.BaseAddress = new Uri("https://localhost:7213/"); // Cambia esto si usas otro puerto

})
.ConfigurePrimaryHttpMessageHandler(() =>
{
    return new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
    };
});

// Registro de servicios
builder.Services.AddScoped<Proyecto.WebMVC.Services.RegionService>();
builder.Services.AddScoped<Proyecto.WebMVC.Services.ComunaService>();


// ?? Agrega soporte para controladores y vistas
builder.Services.AddControllersWithViews();

var app = builder.Build();

// ?? Middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Region}/{action=Index}/{id?}");

app.Run();
