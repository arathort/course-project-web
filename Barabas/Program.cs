using Barabas.Data;
using Barabas.Repositories.EventCategoryRepository;
using Barabas.Repositories.EventRepository;
using Barabas.Repositories.OrderRepository;
using Barabas.Repositories.TicketRepository;
using Barabas.Repositories.VerifyRepository;
using Barabas.Services.EventCategoryService;
using Barabas.Services.EventService;
using Barabas.Services.OrderService;
using Barabas.Services.TicketService;
using Barabas.Services.VerificationService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString)
           .LogTo(Console.WriteLine, LogLevel.Information));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IEventCategoryRepository, EventCategoryRepository>();
builder.Services.AddScoped<IEventCategoryService, EventCategoryService>();
builder.Services.AddScoped<ITicketRepository, TicketRepository>();
builder.Services.AddScoped<ITicketService, TicketService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IVerifyRepository, VerifyRepository>();
builder.Services.AddScoped<IVerificationService, VerificationService>();


builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()  
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Events.OnSignedIn = async context =>
    {
        var userManager = context.HttpContext.RequestServices.GetRequiredService<UserManager<IdentityUser>>();
        var user = await userManager.GetUserAsync(context.Principal);
        var roles = await userManager.GetRolesAsync(user);

        foreach (var role in roles)
        {
            Console.WriteLine("Role: " + role);
        }

        var response = context.HttpContext.Response;

        if (roles.Contains("Admin"))
        {
            response.Redirect("/Admin/dashboard");
            return;
        }
        else if (roles.Contains("Manager"))
        {
            response.Redirect("/Manager/Events");
            return;
        }
        else
        {
            var returnUrl = context.Request.Query["returnUrl"].FirstOrDefault();

            if (!string.IsNullOrEmpty(returnUrl))
            {
                response.Redirect(returnUrl);
            }
            else
            {
                response.Redirect("/Home/Index");
            }
        }
    };
});

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
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
    name: "admin",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}");

app.MapControllerRoute(
    name: "manager",
    pattern: "{area:exists}/{controller=Events}/{action=Index}/{id?}");


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
