using Azure.Identity;
using BLL.Infrastructure;
using BLL.Services;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.FileProviders;
using WebBeautyBook.Converter;

var builder = WebApplication.CreateBuilder(args);

//Azure
var keyVaultEndpoint = new Uri(Environment.GetEnvironmentVariable("VaultUri"));
builder.Configuration.AddAzureKeyVault(keyVaultEndpoint, new DefaultAzureCredential());


//Add Services
builder.Services.AddTransient<ClientService>();
builder.Services.AddTransient<BaseUserService>();
builder.Services.AddTransient<CommentService>();
builder.Services.AddTransient<LocationService>();
builder.Services.AddTransient<RecordService>();
builder.Services.AddTransient<ScheduleService>();
builder.Services.AddTransient<ServiceService>();
builder.Services.AddTransient<WorkerService>();
builder.Services.AddTransient<WorkerServiceService>();
builder.Services.AddTransient<CategoryService>();
builder.Services.AddTransient<CompanyService>();
builder.Services.AddTransient<CompanyOpenHoursService>();


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
    opt.SenderEmail = "logologi417@gmail.com";
    opt.SenderName = "Tima";
});


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

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
            Path.Combine(app.Environment.ContentRootPath, "wwwroot")),
    RequestPath = "/wwwroot"
});

app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();
app.UseHttpsRedirection();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.MapFallbackToFile("index.html"); ;

app.Run();