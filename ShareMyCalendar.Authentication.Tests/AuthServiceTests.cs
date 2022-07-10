using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moq;
using ShareMyCalendar.Authentication.Data;
using ShareMyCalendar.Authentication.Requests;
using ShareMyCalendar.Authentication.Services;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace ShareMyCalendar.Authentication.Tests
{
    public class AuthServiceTests
    {
        private readonly Mock<UserManager<User>> _userManagerMock;
        private readonly Mock<SignInManager<User>> _signInManagerMock;
        private readonly Mock<IAccessTokenGenerator> _accessTokenGeneratorMock;
        private readonly User _user;
        private readonly AuthService _sut;
        private const string _username = "USERNAME_001";
        private const string _password = "PASSWORD_001";
        private const string _accessToken = "ACCESS_TOKEN_001";
        private const string _passwordResetToken = "PASSWORD_RESET_TOKEN_001";
        private static readonly IdentityError[] _errors = new[]
        {
            new IdentityError { Code = "CODE_001", Description = "DESCRIPTION_001" },
            new IdentityError { Code = "CODE_002", Description = "DESCRIPTION_002" },
            new IdentityError { Code = "CODE_003", Description = "DESCRIPTION_003" },
        };

        public AuthServiceTests()
        {
            _userManagerMock = new Mock<UserManager<User>>(
                Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null
            );

            _signInManagerMock = new Mock<SignInManager<User>>(_userManagerMock.Object,
                Mock.Of<IHttpContextAccessor>(),
                Mock.Of<IUserClaimsPrincipalFactory<User>>(),
                null,
                null,
                null,
                null);

            _accessTokenGeneratorMock = new Mock<IAccessTokenGenerator>();

            _user = new User { UserName = _username };

            _sut = new AuthService(_signInManagerMock.Object, _userManagerMock.Object, _accessTokenGeneratorMock.Object);
        }

        [Fact]
        public async Task Login_UserDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            var loginRequest = new LoginRequest();

            // Act
            var result = await _sut.Login(loginRequest);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsT0);
        }

        [Fact]
        public async Task Login_LockedOut_ReturnsCorrectResponseType()
        {
            // Arrange
            _userManagerMock.Setup(x => x.FindByNameAsync(_username)).ReturnsAsync(_user);

            _signInManagerMock
                .Setup(x => x.CheckPasswordSignInAsync(_user, _password, It.IsAny<bool>()))
                .ReturnsAsync(SignInResult.LockedOut);

            // Act
            var result = await _sut.Login(new LoginRequest
            {
                UserName = _username,
                Password = _password,
            });

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsT1);
        }

        [Fact]
        public async Task Login_TwoFactorRequired_ReturnsCorrectResponseType()
        {
            // Arrange
            _userManagerMock.Setup(x => x.FindByNameAsync(_username)).ReturnsAsync(_user);

            _signInManagerMock
                .Setup(x => x.CheckPasswordSignInAsync(_user, _password, It.IsAny<bool>()))
                .ReturnsAsync(SignInResult.TwoFactorRequired);

            // Act
            var result = await _sut.Login(new LoginRequest
            {
                UserName = _username,
                Password = _password,
            });

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsT2);
        }

        [Fact]
        public async Task Login_NotAllowed_ReturnsCorrectResponseType()
        {
            // Arrange
            _userManagerMock.Setup(x => x.FindByNameAsync(_username)).ReturnsAsync(_user);

            _signInManagerMock
                .Setup(x => x.CheckPasswordSignInAsync(_user, _password, It.IsAny<bool>()))
                .ReturnsAsync(SignInResult.NotAllowed);

            // Act
            var result = await _sut.Login(new LoginRequest
            {
                UserName = _username,
                Password = _password,
            });

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsT3);
        }

        [Fact]
        public async Task Login_Failed_ReturnsCorrectResponseType()
        {
            // Arrange
            _userManagerMock.Setup(x => x.FindByNameAsync(_username)).ReturnsAsync(_user);

            _signInManagerMock
                .Setup(x => x.CheckPasswordSignInAsync(_user, _password, It.IsAny<bool>()))
                .ReturnsAsync(SignInResult.Failed);

            // Act
            var result = await _sut.Login(new LoginRequest
            {
                UserName = _username,
                Password = _password,
            });

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsT4);
        }

        [Fact]
        public async Task Login_Success_ReturnsCorrectResponseType()
        {
            // Arrange
            _userManagerMock.Setup(x => x.FindByNameAsync(_username)).ReturnsAsync(_user);

            _signInManagerMock
                .Setup(x => x.CheckPasswordSignInAsync(_user, _password, It.IsAny<bool>()))
                .ReturnsAsync(SignInResult.Success);

            _accessTokenGeneratorMock.Setup(x => x.GenerateAccessToken(It.IsAny<IEnumerable<Claim>>())).Returns(_accessToken);

            // Act
            var result = await _sut.Login(new LoginRequest
            {
                UserName = _username,
                Password = _password,
            });

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsT5);
            Assert.Equal(_username, result.AsT5.UserName);
            Assert.Equal(_accessToken, result.AsT5.AccessToken);
        }

        [Fact]
        public async Task ChangePassword_UserDoesNotExist_ReturnsCorrectResponseType()
        {
            // Act
            var result = await _sut.ChangePassword(new ChangePasswordRequest
            {
                UserName = _username,
                CurrentPassword = _password,
                NewPassword = _password,
            });

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsT0);
        }

        [Fact]
        public async Task ChangePassword_IdentityErrors_ReturnsCorrectResponseType()
        {
            // Arrange
            _userManagerMock.Setup(x => x.FindByNameAsync(_username)).ReturnsAsync(_user);
            _userManagerMock
                .Setup(x => x.ChangePasswordAsync(_user, _password, _password))
                .ReturnsAsync(IdentityResult.Failed(_errors));

            // Act
            var result = await _sut.ChangePassword(new ChangePasswordRequest
            {
                UserName = _username,
                CurrentPassword = _password,
                NewPassword = _password,
            });

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsT1);
            Assert.Equal(_errors, result.AsT1.Errors);
        }

        [Fact]
        public async Task ChangePassword_Success_ReturnsCorrectResponseType()
        {
            // Arrange
            _userManagerMock.Setup(x => x.FindByNameAsync(_username)).ReturnsAsync(_user);
            _userManagerMock
                .Setup(x => x.ChangePasswordAsync(_user, _password, _password))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _sut.ChangePassword(new ChangePasswordRequest
            {
                UserName = _username,
                CurrentPassword = _password,
                NewPassword = _password,
            });

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsT2);
        }

        [Fact]
        public async Task GeneratePasswordResetToken_UserDoesNotExist_ReturnsCorrectResponseType()
        {
            // Act
            var result = await _sut.GeneratePasswordResetToken(_username);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsT0);
        }

        [Fact]
        public async Task GeneratePasswordResetToken_UserExists_ReturnsCorrectResponseType()
        {
            // Arrange
            _userManagerMock.Setup(x => x.FindByNameAsync(_username)).ReturnsAsync(_user);
            _userManagerMock
                .Setup(x => x.GeneratePasswordResetTokenAsync(_user))
                .ReturnsAsync(_passwordResetToken);

            // Act
            var result = await _sut.GeneratePasswordResetToken(_username);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsT1);
            Assert.Equal(_passwordResetToken, result.AsT1);
        }

        [Fact]
        public async Task ResetPassword_UserDoesNotExist_ReturnsCorrectResponseType()
        {
            // Act
            var result = await _sut.ResetPassword(new ResetPasswordRequest
            {
                UserName = _username,
                Password = _password,
                Token = _passwordResetToken
            });

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsT0);
        }

        [Fact]
        public async Task ResetPassword_IdentityError_ReturnsCorrectResponseType()
        {
            // Arrange
            _userManagerMock.Setup(x => x.FindByNameAsync(_username)).ReturnsAsync(_user);
            _userManagerMock
                .Setup(x => x.ResetPasswordAsync(_user, _passwordResetToken, _password))
                .ReturnsAsync(IdentityResult.Failed(_errors));

            // Act
            var result = await _sut.ResetPassword(new ResetPasswordRequest
            {
                UserName = _username,
                Password = _password,
                Token = _passwordResetToken
            });

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsT1);
            Assert.Equal(_errors, result.AsT1.Errors);
        }

        [Fact]
        public async Task ResetPassword_Success_ReturnsCorrectResponseType()
        {
            // Arrange
            _userManagerMock.Setup(x => x.FindByNameAsync(_username)).ReturnsAsync(_user);
            _userManagerMock
                .Setup(x => x.ResetPasswordAsync(_user, _passwordResetToken, _password))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _sut.ResetPassword(new ResetPasswordRequest
            {
                UserName = _username,
                Password = _password,
                Token = _passwordResetToken
            });

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsT2);
        }
    }
}