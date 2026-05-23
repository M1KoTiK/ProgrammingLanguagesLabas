var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapPost("/greet", (HttpRequest request) =>
{
    var username = request.Form["username"].FirstOrDefault();

    if (string.IsNullOrWhiteSpace(username))
    {
        return Results.Content("<h2>Имя не указано</h2><a href='/'>Назад</a>", "text/html; charset=utf-8");
    }

    return Results.Content($"<h2>Привет, {username}!</h2><a href='/'>Назад</a>", "text/html; charset=utf-8");
});

app.Run();