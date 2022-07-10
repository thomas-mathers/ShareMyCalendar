using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ShareMyCalendar.Authentication.Data;
using ShareMyCalendar.Authentication.Options;
using ShareMyCalendar.Authentication.Requests;
using ShareMyCalendar.Authentication.Services;
using ShareMyCalendar.Authentication.Tests.Comparers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ShareMyCalendar.Authentication.Tests
{
    public class UserServiceIntegrationTests
    {
        private readonly UserManager<User> _userManager;
        private readonly IUserService _sut;
        private const string _username = "thomas.mathers.pro@gmail.com";
        private const string _password1 = "P@sSw0rd1!";
        private const string _password2 = "P@sSw0rd2!";
        private readonly User _user = new() { UserName = _username, Email = _username };

        public UserServiceIntegrationTests()
        {
            var services = new ServiceCollection();
            services.Configure<AccessTokenGeneratorOptions>(options =>
            {
                options.Issuer = "share-my-calendar";
                options.Audience = "share-my-calendar";
                options.Key = "share-my-calendar-signing-key-001";
                options.LifespanInDays = 1;
            });
            services.AddDbContext<DatabaseContext>(x => x.UseInMemoryDatabase(Guid.NewGuid().ToString()));
            services
                .AddIdentity<User, Role>(options =>
                {
                    options.User.RequireUniqueEmail = true;
                })
                .AddEntityFrameworkStores<DatabaseContext>()
                .AddDefaultTokenProviders();
            services.AddScoped<IAccessTokenGenerator, AccessTokenGenerator>();
            services.AddScoped<IUserService, UserService>();
            services.AddLogging();
            var serviceProvider = services.BuildServiceProvider();
            _userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            _sut = serviceProvider.GetRequiredService<IUserService>();
        }

        [Theory]
        [InlineData("")]
        [InlineData("a@")]
        public async Task Register_InvalidUsername_ReturnsIdentityErrorResponse(string username)
        {
            // Arrange
            var expectedErrors = new List<IdentityError>
            {
                new IdentityError
                {
                    Code = "InvalidEmail"
                }
            };

            // Act
            var registerResponse = await _sut.Register(new RegisterRequest
            {
                UserName = username,
                Password = _password1
            });

            // Assert
            Assert.NotNull(registerResponse);
            Assert.True(registerResponse.IsT0);
            Assert.Equal(expectedErrors, registerResponse.AsT0.Errors, new IdentityErrorComparer());
        }
    }
}
