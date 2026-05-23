var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapPost("/time", (HttpRequest request) =>
{
    var timezoneId = request.Form["timezone"].FirstOrDefault();

    var timezone = TimeZoneInfo.FindSystemTimeZoneById(timezoneId);
    var utcNow = DateTime.UtcNow;
    var localTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, timezone);

    Thread.Sleep(5000);

    var html = $"<h2>{timezone.DisplayName}</h2>" +
               $"<p>Время: {localTime:HH:mm:ss}</p>" +
               $"<p>Дата: {localTime:dd.MM.yyyy}</p>" +
               $"<p>UTC: {utcNow:HH:mm:ss}</p>" +
               $"<p><i>(ответ с задержкой 5 секунд)</i></p>" +
               $"<a href='/'>Назад</a>";

    return Results.Content(html, "text/html; charset=utf-8");
});

app.Run();