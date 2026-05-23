using Microsoft.EntityFrameworkCore;
using laba12.Data;
using laba12.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddDbContext<UsersDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<UsersDbContext>();

    context.Users.RemoveRange(context.Users);
    context.Roles.RemoveRange(context.Roles);
    context.SaveChanges();

    var adminRole = new Role { Name = "admin" };
    var userRole = new Role { Name = "user" };
    context.Roles.AddRange(adminRole, userRole);
    context.SaveChanges();

    var admin = new User
    {
        Login = "admin",
        Password = "password",
        Balance = 0,
        RoleId = adminRole.Id
    };
    var user1 = new User
    {
        Login = "ivanov",
        Password = "qwerty",
        Balance = 500,
        RoleId = userRole.Id
    };
    var user2 = new User
    {
        Login = "petrov",
        Password = "123456",
        Balance = 300,
        RoleId = userRole.Id
    };
    context.Users.AddRange(admin, user1, user2);
    context.SaveChanges();
}
app.Run();