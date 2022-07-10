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
    public class AuthServiceIntegrationTests
    {
        private readonly UserManager<User> _userManager;
        private readonly IAuthService _sut;
        private const string _username = "thomas.mathers.pro@gmail.com";
        private const string _password1 = "P@sSw0rd1!";
        private const string _password2 = "P@sSw0rd2!";
        private readonly User _user = new() { UserName = _username, Email = _username };

        public AuthServiceIntegrationTests()
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
            services.AddScoped<IAuthService, AuthService>();
            services.AddLogging();
            var serviceProvider = services.BuildServiceProvider();
            _userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            _sut = serviceProvider.GetRequiredService<IAuthService>();
        }

        [Fact]
        public async Task Login_UserDoesNotExist_ReturnsNotFoundResponse()
        {
            // Act
            var loginResponse = await _sut.Login(new LoginRequest
            {
                UserName = _username,
                Password = _password1
            });

            // Assert
            Assert.NotNull(loginResponse);
            Assert.True(loginResponse.IsT0);
        }

        [Fact]
        public async Task Login_WrongPassword_ReturnsFailureResponse()
        {
            // Arrange
            await _userManager.CreateAsync(_user, _password2);

            // Act
            var loginResponse = await _sut.Login(new LoginRequest
            {
                UserName = _username,
                Password = _password1
            });

            // Assert
            Assert.NotNull(loginResponse);
            Assert.True(loginResponse.IsT4);
        }

        [Fact]
        public async Task Login_MultipleWrongPasswords_ReturnsLockedOutResponse()
        {
            // Arrange
            await _userManager.CreateAsync(_user, _password2);

            var loginRequest = new LoginRequest
            {
                UserName = _username,
                Password = _password1
            };

            for (var i = 0; i < 5; i++)
            {
                await _sut.Login(loginRequest);
            }

            // Act
            var loginResponse = await _sut.Login(loginRequest);

            // Assert
            Assert.NotNull(loginResponse);
            Assert.True(loginResponse.IsT1);
        }

        [Fact]
        public async Task Login_CorrectPassword_ReturnsSuccessResponse()
        {
            // Arrange
            await _userManager.CreateAsync(_user, _password1);

            // Act
            var loginResponse = await _sut.Login(new LoginRequest
            {
                UserName = _username,
                Password = _password1
            });

            // Assert
            Assert.NotNull(loginResponse);
            Assert.True(loginResponse.IsT5);
        }

        [Fact]
        public async Task ChangePassword_UserDoesNotExist_ReturnsNotFoundResponse()
        {
            // Act
            var changePassword = await _sut.ChangePassword(new ChangePasswordRequest
            {
                UserName = _username,
                CurrentPassword = _password1,
                NewPassword = _password2
            });

            // Assert
            Assert.NotNull(changePassword);
            Assert.True(changePassword.IsT0);
        }

        [Fact]
        public async Task ChangePassword_WrongPassword_ReturnsIdentityErrorResponse()
        {
            // Arrange
            await _userManager.CreateAsync(_user, _password1);

            // Act
            var changePassword = await _sut.ChangePassword(new ChangePasswordRequest
            {
                UserName = _username,
                CurrentPassword = _password2,
                NewPassword = _password2
            });

            // Assert
            Assert.NotNull(changePassword);
            Assert.True(changePassword.IsT1);
        }

        [Theory]
        [InlineData("aB(1")]
        [InlineData("aB(12")]
        public async Task ChangePassword_ShortPassword_ReturnsIdentityErrorResponse(string newPassword)
        {
            // Arrange
            await _userManager.CreateAsync(_user, _password1);

            var expectedErrors = new List<IdentityError>
            { 
                new IdentityError 
                { 
                    Code = "PasswordTooShort"
                } 
            };

            // Act
            var changePassword = await _sut.ChangePassword(new ChangePasswordRequest
            {
                UserName = _username,
                CurrentPassword = _password1,
                NewPassword = newPassword
            });

            // Assert
            Assert.NotNull(changePassword);
            Assert.True(changePassword.IsT1);
            Assert.Equal(expectedErrors, changePassword.AsT1.Errors, new IdentityErrorComparer());
        }

        [Theory]
        [InlineData("aB(def")]
        public async Task ChangePassword_NoNumber_ReturnsIdentityErrorResponse(string newPassword)
        {
            // Arrange
            await _userManager.CreateAsync(_user, _password1);

            var expectedErrors = new List<IdentityError>
            {
                new IdentityError
                {
                    Code = "PasswordRequiresDigit"
                }
            };

            // Act
            var changePassword = await _sut.ChangePassword(new ChangePasswordRequest
            {
                UserName = _username,
                CurrentPassword = _password1,
                NewPassword = newPassword
            });

            // Assert
            Assert.NotNull(changePassword);
            Assert.True(changePassword.IsT1);
            Assert.Equal(expectedErrors, changePassword.AsT1.Errors, new IdentityErrorComparer());
        }

        [Theory]
        [InlineData("a2345@")]
        public async Task ChangePassword_NoUpperLetter_ReturnsIdentityErrorResponse(string newPassword)
        {
            // Arrange
            await _userManager.CreateAsync(_user, _password1);

            var expectedErrors = new List<IdentityError>
            {
                new IdentityError
                {
                    Code = "PasswordRequiresUpper"
                }
            };

            // Act
            var changePassword = await _sut.ChangePassword(new ChangePasswordRequest
            {
                UserName = _username,
                CurrentPassword = _password1,
                NewPassword = newPassword
            });

            // Assert
            Assert.NotNull(changePassword);
            Assert.True(changePassword.IsT1);
            Assert.Equal(expectedErrors, changePassword.AsT1.Errors, new IdentityErrorComparer());
        }

        [Theory]
        [InlineData("A2345@")]
        public async Task ChangePassword_NoLowerLetter_ReturnsIdentityErrorResponse(string newPassword)
        {
            // Arrange
            await _userManager.CreateAsync(_user, _password1);

            var expectedErrors = new List<IdentityError>
            {
                new IdentityError
                {
                    Code = "PasswordRequiresLower"
                }
            };

            // Act
            var changePassword = await _sut.ChangePassword(new ChangePasswordRequest
            {
                UserName = _username,
                CurrentPassword = _password1,
                NewPassword = newPassword
            });

            // Assert
            Assert.NotNull(changePassword);
            Assert.True(changePassword.IsT1);
            Assert.Equal(expectedErrors, changePassword.AsT1.Errors, new IdentityErrorComparer());
        }

        [Theory]
        [InlineData("aB3456")]
        public async Task ChangePassword_NoSpecialCharacter_ReturnsIdentityErrorResponse(string newPassword)
        {
            // Arrange
            await _userManager.CreateAsync(_user, _password1);

            var expectedErrors = new List<IdentityError>
            {
                new IdentityError
                {
                    Code = "PasswordRequiresNonAlphanumeric"
                }
            };

            // Act
            var changePassword = await _sut.ChangePassword(new ChangePasswordRequest
            {
                UserName = _username,
                CurrentPassword = _password1,
                NewPassword = newPassword
            });

            // Assert
            Assert.NotNull(changePassword);
            Assert.True(changePassword.IsT1);
            Assert.Equal(expectedErrors, changePassword.AsT1.Errors, new IdentityErrorComparer());
        }

        [Theory]
        [InlineData("aB(123")]
        public async Task ChangePassword_ValidNewPassword_ReturnsSuccessResponse(string newPassword)
        {
            // Arrange
            await _userManager.CreateAsync(_user, _password1);

            // Act
            var changePassword = await _sut.ChangePassword(new ChangePasswordRequest
            {
                UserName = _username,
                CurrentPassword = _password1,
                NewPassword = newPassword
            });

            // Assert
            Assert.NotNull(changePassword);
            Assert.True(changePassword.IsT2);
        }

        [Fact]
        public async Task GenerateResetPasswordToken_UserDoesNotExist_ReturnsNotFoundResponse()
        {
            // Act
            var changePassword = await _sut.GeneratePasswordResetToken(_username);

            // Assert
            Assert.NotNull(changePassword);
            Assert.True(changePassword.IsT0);
        }

        [Fact]
        public async Task GenerateResetPasswordToken_UserExists_ReturnsSuccessResponse()
        {
            // Arrange
            await _userManager.CreateAsync(_user, _password1);

            // Act
            var changePassword = await _sut.GeneratePasswordResetToken(_username);

            // Assert
            Assert.NotNull(changePassword);
            Assert.True(changePassword.IsT1);
            Assert.NotEmpty(changePassword.AsT1);
        }

        [Fact]
        public async Task ResetPassword_UserDoesNotExist_ReturnsNotFoundResponse()
        {
            // Act
            var resetPassword = await _sut.ResetPassword(new ResetPasswordRequest
            {
                UserName = _username,
                Token = string.Empty,
                Password = _password2,
            });

            // Assert
            Assert.NotNull(resetPassword);
            Assert.True(resetPassword.IsT0);
        }

        [Theory]
        [InlineData("")]
        [InlineData("abc123")]
        public async Task ResetPassword_InvalidToken_ReturnsIdentityErrorResponse(string token)
        {
            // Arrange
            await _userManager.CreateAsync(_user, _password1);

            var expectedErrors = new List<IdentityError>
            {
                new IdentityError
                {
                    Code = "InvalidToken"
                }
            };

            // Act
            var resetPassword = await _sut.ResetPassword(new ResetPasswordRequest
            {
                UserName = _username,
                Token = token,
                Password = _password2,
            });

            // Assert
            Assert.NotNull(resetPassword);
            Assert.True(resetPassword.IsT1);
            Assert.Equal(expectedErrors, resetPassword.AsT1.Errors, new IdentityErrorComparer());
        }

        [Theory]
        [InlineData("aB(1")]
        [InlineData("aB(12")]
        public async Task ResetPassword_ShortPassword_ReturnsIdentityErrorResponse(string newPassword)
        {
            // Arrange
            await _userManager.CreateAsync(_user, _password1);
            
            var token = await _userManager.GeneratePasswordResetTokenAsync(_user);

            var expectedErrors = new List<IdentityError>
            {
                new IdentityError
                {
                    Code = "PasswordTooShort"
                }
            };

            // Act
            var resetPassword = await _sut.ResetPassword(new ResetPasswordRequest
            {
                UserName = _username,
                Token = token,
                Password = newPassword
            });

            // Assert
            Assert.NotNull(resetPassword);
            Assert.True(resetPassword.IsT1);
            Assert.Equal(expectedErrors, resetPassword.AsT1.Errors, new IdentityErrorComparer());
        }

        [Theory]
        [InlineData("aB(def")]
        public async Task ResetPassword_NoNumber_ReturnsIdentityErrorResponse(string newPassword)
        {
            // Arrange
            await _userManager.CreateAsync(_user, _password1);

            var token = await _userManager.GeneratePasswordResetTokenAsync(_user);

            var expectedErrors = new List<IdentityError>
            {
                new IdentityError
                {
                    Code = "PasswordRequiresDigit"
                }
            };

            // Act
            var resetPassword = await _sut.ResetPassword(new ResetPasswordRequest
            {
                UserName = _username,
                Token = token,
                Password = newPassword
            });

            // Assert
            Assert.NotNull(resetPassword);
            Assert.True(resetPassword.IsT1);
            Assert.Equal(expectedErrors, resetPassword.AsT1.Errors, new IdentityErrorComparer());
        }

        [Theory]
        [InlineData("a2345@")]
        public async Task ResetPassword_NoUpperLetter_ReturnsIdentityErrorResponse(string newPassword)
        {
            // Arrange
            await _userManager.CreateAsync(_user, _password1);

            var token = await _userManager.GeneratePasswordResetTokenAsync(_user);

            var expectedErrors = new List<IdentityError>
            {
                new IdentityError
                {
                    Code = "PasswordRequiresUpper"
                }
            };

            // Act
            var resetPassword = await _sut.ResetPassword(new ResetPasswordRequest
            {
                UserName = _username,
                Token = token,
                Password = newPassword
            });

            // Assert
            Assert.NotNull(resetPassword);
            Assert.True(resetPassword.IsT1);
            Assert.Equal(expectedErrors, resetPassword.AsT1.Errors, new IdentityErrorComparer());
        }

        [Theory]
        [InlineData("A2345@")]
        public async Task ResetPassword_NoLowerLetter_ReturnsIdentityErrorResponse(string newPassword)
        {
            // Arrange
            await _userManager.CreateAsync(_user, _password1);

            var token = await _userManager.GeneratePasswordResetTokenAsync(_user);

            var expectedErrors = new List<IdentityError>
            {
                new IdentityError
                {
                    Code = "PasswordRequiresLower"
                }
            };

            // Act
            var resetPassword = await _sut.ResetPassword(new ResetPasswordRequest
            {
                UserName = _username,
                Token = token,
                Password = newPassword
            });

            // Assert
            Assert.NotNull(resetPassword);
            Assert.True(resetPassword.IsT1);
            Assert.Equal(expectedErrors, resetPassword.AsT1.Errors, new IdentityErrorComparer());
        }

        [Theory]
        [InlineData("aB3456")]
        public async Task ResetPassword_NoSpecialCharacter_ReturnsIdentityErrorResponse(string newPassword)
        {
            // Arrange
            await _userManager.CreateAsync(_user, _password1);

            var token = await _userManager.GeneratePasswordResetTokenAsync(_user);

            var expectedErrors = new List<IdentityError>
            {
                new IdentityError
                {
                    Code = "PasswordRequiresNonAlphanumeric"
                }
            };

            // Act
            var resetPassword = await _sut.ResetPassword(new ResetPasswordRequest
            {
                UserName = _username,
                Token = token,
                Password = newPassword
            });

            // Assert
            Assert.NotNull(resetPassword);
            Assert.True(resetPassword.IsT1);
            Assert.Equal(expectedErrors, resetPassword.AsT1.Errors, new IdentityErrorComparer());
        }

        [Fact]
        public async Task ResetPassword_ValidTokenValidPassword_ReturnsSuccessResponse()
        {
            // Arrange
            await _userManager.CreateAsync(_user, _password1);
            var token = await _userManager.GeneratePasswordResetTokenAsync(_user);

            // Act
            var resetPassword = await _sut.ResetPassword(new ResetPasswordRequest
            {
                UserName = _username,
                Token = token,
                Password = _password2,
            });

            // Assert
            Assert.NotNull(resetPassword);
            Assert.True(resetPassword.IsT2);
        }
    }
}
