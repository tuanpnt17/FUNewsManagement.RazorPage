using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using PhamNguyenTrongTuanRazorPages.Hubs;
using PhamNguyenTrongTuanRazorPages.Models.Account;
using Repository.Accounts;
using Repository.Categories;
using Repository.NewsArticles;
using Repository.Tags;
using ServiceLayer.Account;
using ServiceLayer.Category;
using ServiceLayer.NewsArticle;
using ServiceLayer.Tag;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var config = builder.Configuration;

services.AddRazorPages();
services.AddDbContext<FuNewsDbContext>(options =>
    options.UseSqlServer(config.GetConnectionString("DefaultConnection"))
);

services.AddSignalR();
services.AddAutoMapper(
    opt =>
    {
        opt.AddProfile<MappingProfile>();
    },
    typeof(AppDomain)
);
services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
        options.SlidingExpiration = true;
    });

services.Configure<AdminOptions>(config.GetSection(AdminOptions.Admin));
services.Configure<PaginationOptions>(config.GetSection(PaginationOptions.Pagination));
services.AddScoped<IAccountService, AccountService>();
services.AddScoped<IAccountRepository, AccountRepository>();
services.AddScoped<INewsArticleService, NewsArticleService>();
services.AddScoped<INewsArticleRepository, NewsArticleRepository>();
services.AddScoped<ICategoryRepository, CategoryRepository>();
services.AddScoped<ICategoryService, CategoryService>();
services.AddScoped<ITagService, TagService>();
services.AddScoped<ITagRepository, TagRepository>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();
app.MapFallbackToPage("/NewsArticle/Index");

app.MapHub<SignalRServer>("/signalrserver");

app.Run();
