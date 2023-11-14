using AspNetCoreRateLimit;
using Azure.Identity;
using BLL.Infrastructure;
using BLL.Services;
using Microsoft.AspNetCore.Identity.UI.Services;
using WebBeautyBook.Converter;

var builder = WebApplication.CreateBuilder(args);

//Azure
var keyVaultEndpoint = new Uri(Environment.GetEnvironmentVariable("VaultUri"));
builder.Configuration.AddAzureKeyVault(keyVaultEndpoint, new DefaultAzureCredential());

//Add Services
builder.Services.AddTransient<BaseUserService>();
builder.Services.AddTransient<CommentService>();
builder.Services.AddTransient<LocationService>();
builder.Services.AddTransient<AppointmentService>();
builder.Services.AddTransient<ReservationService>();
builder.Services.AddTransient<ServiceService>();
builder.Services.AddTransient<WorkerService>();
builder.Services.AddTransient<AssignmentService>();
builder.Services.AddTransient<CategoryService>();
builder.Services.AddTransient<CompanyService>();
builder.Services.AddTransient<CompanyOpenHoursService>();
builder.Services.AddTransient<CompanyImageService>();
builder.Services.AddTransient<CompanyLikeService>();

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.AddMvc().AddDataAnnotationsLocalization();

//Fixes looping navigation fields.
//[JsonIgnore]
///A possible object cycle was detected. This can either be due to a cycle or if the object depth is larger than the maximum allowed depth of 32. 
builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        options.SerializerSettings.Converters.Add(new TimeOnlyJsonConverter());
    });

//Set SendGridEmailSenderOption and Service
builder.Services.AddTransient<IEmailSender, SendGridEmailService>();
builder.Services.Configure<Domain.Models.SendGridEmailSenderOption>(opt =>
{
    opt.ApiKey = builder.Configuration.GetValue<string>("SGKey");
    opt.SenderEmail = builder.Configuration.GetValue<string>("SGKeyEmail");
    opt.SenderName = "Tima";
});

// Set IpRateLimitOptions
builder.Services.AddMemoryCache();
builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
builder.Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
builder.Services.AddInMemoryRateLimiting();

builder.Services.Configure(builder.Configuration);

// Add services to the container.
builder.Services.AddControllersWithViews();

//add swagger
string apiVersion = "v1";
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc(apiVersion,
        new Microsoft.OpenApi.Models.OpenApiInfo()
        {
            Title = "BeautyBookApi",
            Description = $"API {apiVersion}",
            Version = apiVersion
        });
});

var app = builder.Build();
var supportedCultures = new[] { "en", "ru", "pl", "ua", "de" };
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture("en").AddSupportedCultures(supportedCultures).AddSupportedUICultures(supportedCultures);
app.UseRequestLocalization(localizationOptions);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
//add swagger UI
app.UseSwagger();
app.UseSwaggerUI(opt =>
{
    opt.SwaggerEndpoint($"/swagger/{apiVersion}/swagger.json", "BeautyBookApi");
});

app.UseIpRateLimiting();

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.MapFallbackToFile("index.html");

app.Run();