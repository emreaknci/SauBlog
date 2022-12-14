using System.Text;
using Business;
using Core;
using Core.Utilities.Security.JWT;
using DataAccess;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using NToastNotify;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddMvcCore().AddNToastNotifyToastr(new ToastrOptions()
{
    ProgressBar = true,
    TimeOut = 3000,
    PositionClass =ToastPositions.TopLeft 
});
builder.Services.AddCoreService();
builder.Services.AddBusinessService();
builder.Services.AddDataAccessService(builder.Configuration);
builder.Services.AddHttpClient();
TokenOptions? tokenOptions = builder.Configuration.GetSection("TokenOptions").Get<TokenOptions>();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(opts =>
    {
        opts.Cookie.Name = $".sauBlog.auth";   // todo :degistir.
        opts.AccessDeniedPath = "/Auth/AccessDenied";
        opts.LogoutPath= "/Auth/LogOut";
        opts.LoginPath = "/Auth/LogIn";
        opts.SlidingExpiration = true;
    }).AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = tokenOptions.Issuer,
            ValidAudience = tokenOptions.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOptions.SecurityKey))
        };
    });
builder.Services.AddSession(options =>
{
    options.Cookie.Name = $"sauBlog.session"; // todo : degistir.
    options.IdleTimeout = TimeSpan.FromMinutes(180);
    options.Cookie.IsEssential = true;
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
