using Microsoft.EntityFrameworkCore;
using NToastNotify;
using Microsoft.AspNetCore.Identity;
using NBE_Project.Models;
using NBE_Project.Services;
using NBE_Project.GuestRepositary;
using NBE_Project.UserRepositary;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDBContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseSqlServer(connectionString);
});



// Register Identity services
//builder.Services.AddIdentity<AppUser, IdentityRole>()
//                .AddEntityFrameworkStores<ApplicationDBContext>()
//                .AddDefaultTokenProviders();

// Add custom services
builder.Services.AddTransient<INotificationService, NotificationService>();


// Configure NToastNotify
builder.Services.AddMvc().AddNToastNotifyToastr(new ToastrOptions()
{
    ProgressBar = true,
    PreventDuplicates = true,
    CloseButton = true,
    PositionClass = ToastPositions.TopCenter,
});
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
builder.Services.AddScoped<IUserRepo, UserRepo>();
builder.Services.AddScoped<IGuestRepo, GuestRepo>();
builder.Services.AddScoped<IMailHelper, MailHelper>();

builder.Services.AddIdentity<AppUser, IdentityRole>(
  options =>
  {
      options.Password.RequireNonAlphanumeric = true;
      options.Password.RequireDigit = true;
      options.Password.RequireLowercase = true;
      options.Password.RequireUppercase = true;
      options.Password.RequiredLength = 8;
  }
  ).AddEntityFrameworkStores<ApplicationDBContext>()
 .AddDefaultTokenProviders();

// Configure logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=VendorHome}/{action=Index}/{id?}");

app.Run();









