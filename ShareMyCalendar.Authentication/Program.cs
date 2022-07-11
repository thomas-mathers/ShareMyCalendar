using ShareMyCalendar.Authentication.Extensions;
using ShareMyCalendar.Authentication.Settings;

var builder = WebApplication.CreateBuilder(args);

var appSettings = AppSettings.FromConfigurationSection(builder.Configuration.GetSection("AppSettings"));

builder.Services.AddShareMyCalendar(appSettings);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
