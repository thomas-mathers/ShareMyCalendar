﻿using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using ShareMyCalendar.Authentication.Data;
using ShareMyCalendar.Authentication.Requests;
using ShareMyCalendar.Authentication.Services;
using ShareMyCalendar.Authentication.Tests.Comparers;
using ShareMyCalendar.Authentication.Tests.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ShareMyCalendar.Authentication.Tests
{
    public class AuthServiceIntegrationTests
    {
        private readonly UserManager<User> _userManager;
        private readonly IAuthService _sut;
        private const string _username = "tmathers";
        private const string _email = "thomas.mathers.pro@gmail.com";
        private const string _password1 = "P@sSw0rd1!";
        private const string _password2 = "P@sSw0rd2!";
        private readonly User _user = new() { UserName = _username, Email = _email };

        public AuthServiceIntegrationTests()
        {
            var serviceProvider = ShareMyCalendarServiceProvider.Build();

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
        [InlineData("aB(1", "PasswordTooShort")]
        [InlineData("aB(12", "PasswordTooShort")]
        [InlineData("aB(def", "PasswordRequiresDigit")]
        [InlineData("a2345@", "PasswordRequiresUpper")]
        [InlineData("A2345@", "PasswordRequiresLower")]
        [InlineData("aB3456", "PasswordRequiresNonAlphanumeric")]
        public async Task ChangePassword_InvalidPassword_ReturnsIdentityErrorResponse(string newPassword, string expectedValidationError)
        {
            // Arrange
            await _userManager.CreateAsync(_user, _password1);

            var expectedErrors = new List<IdentityError>
            {
                new IdentityError
                {
                    Code = expectedValidationError
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
        public async Task GenerateResetPasswordToken_UserExists_ReturnsSuccessResponse()
        {
            // Arrange
            await _userManager.CreateAsync(_user, _password1);

            // Act
            var token = await _sut.GeneratePasswordResetToken(_user);

            // Assert
            Assert.NotNull(token);
            Assert.NotEmpty(token);
        }

        [Fact]
        public async Task ResetPassword_UserDoesNotExist_ReturnsNotFoundResponse()
        {
            // Act
            var resetPassword = await _sut.ChangePassword(new ChangePasswordRequest
            {
                UserName = _username,
                Token = string.Empty,
                NewPassword = _password2,
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
            var resetPassword = await _sut.ChangePassword(new ChangePasswordRequest
            {
                UserName = _username,
                Token = token,
                NewPassword = _password2,
            });

            // Assert
            Assert.NotNull(resetPassword);
            Assert.True(resetPassword.IsT1);
            Assert.Equal(expectedErrors, resetPassword.AsT1.Errors, new IdentityErrorComparer());
        }

        [Theory]
        [InlineData("aB(1", "PasswordTooShort")]
        [InlineData("aB(12", "PasswordTooShort")]
        [InlineData("aB(def", "PasswordRequiresDigit")]
        [InlineData("a2345@", "PasswordRequiresUpper")]
        [InlineData("A2345@", "PasswordRequiresLower")]
        [InlineData("aB3456", "PasswordRequiresNonAlphanumeric")]

        public async Task ResetPassword_InvalidPassword_ReturnsIdentityErrorResponse(string newPassword, string expectedValidationError)
        {
            // Arrange
            await _userManager.CreateAsync(_user, _password1);
            
            var token = await _userManager.GeneratePasswordResetTokenAsync(_user);

            var expectedErrors = new List<IdentityError>
            {
                new IdentityError
                {
                    Code = expectedValidationError
                }
            };

            // Act
            var resetPassword = await _sut.ChangePassword(new ChangePasswordRequest
            {
                UserName = _username,
                Token = token,
                NewPassword = newPassword
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
            var resetPassword = await _sut.ChangePassword(new ChangePasswordRequest
            {
                UserName = _username,
                Token = token,
                NewPassword = _password2,
            });

            // Assert
            Assert.NotNull(resetPassword);
            Assert.True(resetPassword.IsT2);
        }
    }
}