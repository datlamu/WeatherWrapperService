using StackExchange.Redis;
using WeatherWrapperService.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure settings
builder.Services.Configure<OpenWeatherSettings>(
    builder.Configuration.GetSection("OpenWeather"));
builder.Services.Configure<CacheSettings>(
    builder.Configuration.GetSection("CacheSettings"));
builder.Services.Configure<RedisSettings>(
    builder.Configuration.GetSection("Redis"));

var redisSettings = builder.Configuration.GetSection("Redis").Get<RedisSettings>();
var redis = ConnectionMultiplexer.Connect(redisSettings!.ConnectionString!);

// Register lifetimes for services
builder.Services.AddSingleton<IConnectionMultiplexer>(redis);
builder.Services.AddSingleton<IRedisService, RedisService>();
builder.Services.AddHttpClient<IWeatherService, WeatherService>();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
