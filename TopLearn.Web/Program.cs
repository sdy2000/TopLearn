using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using TopLearn.Core.Convertors;
using TopLearn.Core.Servises;
using TopLearn.Core.Servises.Interfaces;
using TopLearn.DataLayer.Context;

var builder = WebApplication.CreateBuilder(args);

//
// FOR UPLOAD BIG FILE
//
//builder.Services.Configure<FormOptions>(options =>
//{
//    options.MultipartBodyLengthLimit = 6000000;
//});


#region Authentication

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(options =>
{
    options.LoginPath="/Login";
    options.LogoutPath = "/Logout";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(43200);
});

#endregion


#region DataBase Context

//Connection to TopLearn Context
var connectionString = builder.Configuration.GetConnectionString("TopLearnConnection");
builder.Services.AddDbContext<TopLearnContext>(x => x.UseSqlServer(connectionString));

#endregion


#region IoC

builder.Services.AddTransient<IUserService,ViewRenderService>();
builder.Services.AddTransient<IViewRenderService, RenderViewToString>();
builder.Services.AddTransient<IPermissionService, PermissionService>();
builder.Services.AddTransient<ICourseService, CourseService>();
builder.Services.AddTransient<IOrderService, OrderService>();

#endregion



// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();



var app = builder.Build();

app.Use(async (context, next) =>
{
    if (context.Request.Path.Value.ToString().ToLower().StartsWith("/couresefilesonline"))
    {
        var callingUrl = context.Request.Headers["Referer"].ToString();
        if (callingUrl != "" && (callingUrl.StartsWith("https://http://localhost:5114/") || callingUrl.StartsWith("http://http://localhost:5114/")))
        {
            await next.Invoke();
        }
        else
        {
            context.Response.Redirect("/Login");
        }
    }
    else
    {
        await next.Invoke();
    }
});


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();
app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();




app.MapRazorPages();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(

    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
