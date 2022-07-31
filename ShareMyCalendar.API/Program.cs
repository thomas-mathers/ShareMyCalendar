using FluentValidation.AspNetCore;
using System.Reflection;
using System.Text.Json.Serialization;
using ThomasMathers.Infrastructure.ResponseWrapping.Extensions;
using ThomasMathers.Infrastructure.Email.Extensions;
using ThomasMathers.Infrastructure.IAM.Emails.Extensions;
using ThomasMathers.Infrastructure.IAM.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();

builder.Services.AddEmailService(builder.Configuration);
builder.Services.AddIamEmails(builder.Configuration);
builder.Services.AddIam(builder.Configuration);

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
builder.Services.AddSwaggerGen();

var app = builder.Build();

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
