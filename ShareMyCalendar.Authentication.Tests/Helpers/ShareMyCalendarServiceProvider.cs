using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using ShareMyCalendar.Authentication.Extensions;
using ShareMyCalendar.Authentication.Settings;
using System;

namespace ShareMyCalendar.Authentication.Tests.Helpers
{
    internal static class ShareMyCalendarServiceProvider
    {
        public static IServiceProvider Build()
        {
            var builder = WebApplication.CreateBuilder();

            var appSettings = AppSettings.FromConfigurationSection(builder.Configuration.GetSection("AppSettings"));

            builder.Services.AddShareMyCalendar(appSettings);

            var serviceProvider = builder.Services.BuildServiceProvider();

            return serviceProvider;
        }
    }
}
