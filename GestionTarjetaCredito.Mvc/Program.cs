using GestionTarjetaCredito.Mvc.Services;
using QuestPDF.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient<CreditCardApiService>(client =>
{
    var apiBaseUrl = builder.Configuration["ApiSettings:BaseUrl"];

    if (string.IsNullOrWhiteSpace(apiBaseUrl))
    {
        throw new InvalidOperationException(
            "No se encontró la configuración 'ApiSettings:BaseUrl'.");
    }

    client.BaseAddress = new Uri(apiBaseUrl);
});

QuestPDF.Settings.License = LicenseType.Community;

var app = builder.Build();

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
    pattern: "{controller=CreditCards}/{action=Statement}/{id?}");

app.Run();