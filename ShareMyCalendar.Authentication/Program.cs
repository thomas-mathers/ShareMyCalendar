using FluentValidation.AspNetCore;
using System.Reflection;
using System.Text.Json.Serialization;
using ThomasMathers.Common.IAM.Extensions;
using ThomasMathers.Common.ResponseWrapping.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddIAM(builder.Configuration);

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

app.UseResponseWrappingExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseIAM();

app.MapControllers();

app.Run();
