﻿using Microsoft.AspNetCore.Identity;
using ShareMyCalendar.Authentication.Data;
using ShareMyCalendar.Authentication.Requests;
using ShareMyCalendar.Authentication.Responses;

namespace ShareMyCalendar.Authentication.Services
{
    public interface IAuthService
    {
        Task<LoginResponse> Login(LoginRequest loginRequest);
        Task<ChangePasswordResponse> ChangePassword(ChangePasswordRequest changePasswordRequest);
        Task<GeneratePasswordResetTokenResponse> GeneratePasswordResetToken(string username);
        Task<ResetPasswordResponse> ResetPassword(ResetPasswordRequest resetPasswordRequest);
    }

    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IAccessTokenGenerator _accessTokenGenerator;

        public AuthService(SignInManager<User> signInManager, UserManager<User> userManager, IAccessTokenGenerator accessTokenGenerator)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _accessTokenGenerator = accessTokenGenerator;
        }

        public async Task<LoginResponse> Login(LoginRequest loginRequest)
        {
            var user = await _userManager.FindByNameAsync(loginRequest.UserName);

            if (user == null)
            {
                return new NotFoundResponse();
            }

            var signInResult = await _signInManager.CheckPasswordSignInAsync(user, loginRequest.Password, true);

            if (!signInResult.Succeeded)
            {
                if (signInResult.IsLockedOut)
                {
                    return new UserLockedOutResponse();
                }

                if (signInResult.RequiresTwoFactor)
                {
                    return new LoginRequiresTwoFactorResponse();
                }

                if (signInResult.IsNotAllowed)
                {
                    return new LoginIsNotAllowedResponse();
                }

                return new LoginFailureResponse();
            }
            
            var claims = await _userManager.GetClaimsAsync(user);

            return new LoginSuccessResponse(user, _accessTokenGenerator.GenerateAccessToken(claims));
        }

        public async Task<ChangePasswordResponse> ChangePassword(ChangePasswordRequest changePasswordRequest)
        {
            var user = await _userManager.FindByNameAsync(changePasswordRequest.UserName);

            if (user == null)
            {
                return new NotFoundResponse();
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(
                user, 
                changePasswordRequest.CurrentPassword, 
                changePasswordRequest.NewPassword);

            if (!changePasswordResult.Succeeded)
            {
                return new IdentityErrorResponse(changePasswordResult.Errors);
            }

            return new ChangePasswordSuccessResponse();
        }

        public async Task<GeneratePasswordResetTokenResponse> GeneratePasswordResetToken(string username)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
            {
                return new NotFoundResponse();
            }

            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }

        public async Task<ResetPasswordResponse> ResetPassword(ResetPasswordRequest resetPasswordRequest)
        {
            var user = await _userManager.FindByNameAsync(resetPasswordRequest.UserName);

            if (user == null)
            {
                return new NotFoundResponse();
            }

            var resetPasswordResult = await _userManager.ResetPasswordAsync(user, resetPasswordRequest.Token, resetPasswordRequest.Password);

            if (!resetPasswordResult.Succeeded)
            {
                return new IdentityErrorResponse(resetPasswordResult.Errors);
            }

            return new ResetPasswordSuccessResponse();
        }
    }
}
