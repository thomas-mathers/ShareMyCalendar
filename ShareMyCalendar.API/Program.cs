using FluentValidation.AspNetCore;

using Microsoft.OpenApi.Models;

using ShareMyCalendar.API;

using System.Reflection;
using System.Text.Json.Serialization;

using ThomasMathers.Infrastructure.Email.Extensions;
using ThomasMathers.Infrastructure.IAM.Emails.Extensions;
using ThomasMathers.Infrastructure.IAM.Extensions;
using ThomasMathers.Infrastructure.ResponseWrapping.Extensions;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Services.AddCors();

builder.Services.AddEmailService(configuration);
builder.Services.AddIamEmails(configuration);
builder.Services.AddIam(configuration);

builder.Services.AddScoped<IDatabaseSeeder, DatabaseSeeder>();

builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
    options.LowercaseQueryStrings = true;
});

builder.Services
    .AddControllers()
    .AddFluentValidation(options =>
    {
        options.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
    })
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

builder.Services.AddResponseWrapping();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "ShareMyCalendar API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[]{}
        }
    });
});

var app = builder.Build();

if (bool.TryParse(configuration["SeedDatabase"], out var seedDatabase) && seedDatabase)
{
    var serviceScopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();

    using (var scope = serviceScopeFactory.CreateScope())
    {
        var seeder = scope.ServiceProvider.GetRequiredService<IDatabaseSeeder>();
        seeder.Seed().Wait();
    }
}

app.UseCors(x =>
{
    x.AllowAnyMethod();
    x.AllowAnyHeader();
    x.SetIsOriginAllowed(origin => true);
    x.AllowCredentials();
});

app.UseResponseWrappingExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseIam();

app.MapControllers();

app.Run();
