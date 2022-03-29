using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using CsvHelper;
using System.Globalization;
using System.Text.Json;
using Microsoft.Net.Http.Headers;

var Configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", optional: false)
        .Build();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddRazorPages();

builder.Services.AddControllers(options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true);

builder.Services.AddDbContext<VortaroContext>();

async Task Validate(TokenValidatedContext context)
{
    var db = context.HttpContext.RequestServices.GetRequiredService<VortaroContext>();
    var u = await db.FindAsync<Uzanto>(context.Principal!.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")!.Value);
    
    if(u is null)
        db.Add(new Uzanto()
        {
            Id = context.Principal!.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")!.Value,
            Nomo = context.Principal!.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")!.Value,
            Bildo = context.Principal!.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/picture")!.Value
        });
    else
    {        
        u.Nomo = context.Principal!.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")!.Value;
        u.Bildo = context.Principal!.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/picture")!.Value;
        db.Update(u);
    }
}

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.RequireAuthenticatedSignIn = false;
    }).AddJwtBearer(options =>
    {
        options.Authority = Configuration["Auth0:Authority"];
        options.Audience = Configuration["Auth0:ApiIdentifier"];
        options.Events = new()
        {
            OnTokenValidated = Validate
        };
    });

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

if(Environment.GetCommandLineArgs().FirstOrDefault(a => a.EndsWith(".csv")) is string s)
{
    using (var serviceScope = app.Services.CreateScope())
    using (var reader = new StreamReader(s))
    using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
    {
        var services = serviceScope.ServiceProvider;
        Krei.KreiDatumbazon(
            csv.GetRecords<CsvModel>(), 
            services.GetRequiredService<VortaroContext>());
    }
    return;
}

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.UseCors(policy => 
    policy.WithOrigins("http://localhost:5000", "https://localhost:5001")
    .AllowAnyMethod()
    .AllowAnyOrigin());

app.Run();