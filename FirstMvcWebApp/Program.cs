using FirstMvcWebApp.Interfaces;
using FirstMvcWebApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure HTTP client for API calls
var apiBaseUrl = builder.Configuration["ApiSettings:BaseUrl"];
builder.Services.AddHttpClient("ApiClient", client =>
{
    client.BaseAddress = new Uri(apiBaseUrl); // Replace with your actual API URL
}).AddHttpMessageHandler<JwtHttpClientHandler>(); // Register custom handler
// Add MVC controllers with views
builder.Services.AddControllersWithViews();

// Add support for accessing HTTP context
builder.Services.AddHttpContextAccessor();

builder.Services.AddTransient<JwtHttpClientHandler>();  // Register custom handler


// Register AccountService with dependency injection
builder.Services.AddScoped<IAccountService, AccountService>();

// Configure distributed memory cache
builder.Services.AddDistributedMemoryCache();

// Enable session support
builder.Services.AddSession(options =>
    {
        options.IdleTimeout = TimeSpan.FromHours(8); // 8-hour session timeout
        options.Cookie.HttpOnly = true;
        options.Cookie.IsEssential = true;
    });

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error"); // Redirect to error handling page in production
    app.UseHsts(); // Enable HTTP Strict Transport Security (HSTS)
}

// Enforce HTTPS redirection
app.UseHttpsRedirection();

// Enable routing middleware
app.UseRouting();

// Enable authorization middleware
app.UseAuthorization();

// Serve static assets (CSS, JS, images, etc.)
app.MapStaticAssets();

// Configure default route for MVC controllers
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

// Enable session middleware
app.UseSession();

// Start the application
app.Run();
