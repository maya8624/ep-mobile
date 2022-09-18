using ep.Mobile.Extensions;
using ep.Mobile.Interfaces.IAPIs;
using ep.Mobile.Interfaces.IServices;
using ep.Mobile.Models;
using ep.Mobile.PageModels.Base;
using ep.Mobile.Reference;
using ep.Mobile.Utils;
using ep.Mobile.Validations;
using ep.Mobile.Views;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using MvvmHelpers.Commands;
using System;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ep.Mobile.PageModels
{
    public class ResetPasswordModel : PageModelBase
    {
        private readonly IAPIService _apiService;
        private readonly IPageService _pageService;

        public AsyncCommand ReceiveCommand { get; }
        public AsyncCommand ResetCommand { get; }
        public AsyncCommand VerifyCommand { get; }
        
        private string _currentPassword;
        public string CurrentPassword 
        { 
            get => _currentPassword;
            set => SetProperty(ref _currentPassword, value);
        }

        private string _newPassword;
        public string NewPassword
        {
            get => _newPassword;
            set => SetProperty(ref _newPassword, value);
        }
        
        private string _confirmPassword;
        public string ConfirmPassword
        {
            get => _confirmPassword;
            set => SetProperty(ref _confirmPassword, value);
        }

        private string _email;
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        private string _verificationCode;
        public string VerificationCode
        {
            get => _verificationCode;
            set => SetProperty(ref _verificationCode, value);
        }

        private bool _showPasswordReset;
        public bool ShowPasswordReset
        {
            get => _showPasswordReset;
            set => SetProperty(ref _showPasswordReset, value);
        }

        private bool _showVerificationCode;
        public bool ShowVerificationCode
        {
            get => _showVerificationCode;
            set => SetProperty(ref _showVerificationCode, value);
        }

        private bool _showReceiveEmail = true;
        public bool ShowReceiveEmail
        {
            get => _showReceiveEmail;
            set => SetProperty(ref _showReceiveEmail, value);
        }

        private string _validateMessage;
        public string ValidateMessage
        {
            get => _validateMessage;
            set => SetProperty(ref _validateMessage, value);
        }

        private readonly ResetPasswordValidation _validation;

        private readonly string _endPoint = "message/email";
        
        public ResetPasswordModel()
        {
            _apiService = DependencyService.Get<IAPIService>();
            _pageService = DependencyService.Get<IPageService>();
            _validation = new ResetPasswordValidation();

            ResetCommand = new AsyncCommand(ResetPassword);
            ReceiveCommand = new AsyncCommand(ReceiveVerificationCode);
            VerifyCommand = new AsyncCommand(VerifyCode);
    }

    private async Task ReceiveVerificationCode()
        {
            try
            {
                //TODO: implement random code generator
                //var code = "123456";
                var code = PasswordGenerator.GenerateVerificationCode();
                await SecureStorage.SetAsync("verification_code", code);
                var shop = await App.Database.GetShopAsync();
                var userEmail = await SecureStorage.GetAsync(Constant.StorageEmailKey);
                
                var email = new EmailForm
                {
                    To = userEmail,
                    Subject = "EP - Verification Code",
                    Content = $"Verification Code: {code}, Enter the 6 digit code on the app"
                };

                //TODO: send an email with 6 digit code
                await _apiService.PostAsync(email, _endPoint);
                _showReceiveEmail = false;
                _showVerificationCode = true;
            }
            catch (Exception ex)
            {
                await _pageService.DisplayAlert("Error ReceiveVerificationCode", ex.Message, "OK");
                throw;
            }
        }

        private async Task VerifyCode()
        {
            if (string.IsNullOrEmpty(VerificationCode))
            {
                await _pageService.DisplayAlert("Warning", "Please enter 6 digits verification code", "OK");
                return;
            }

            var verificationCode = await SecureStorage.GetAsync("verification_code");
            if (string.IsNullOrEmpty(verificationCode) || !verificationCode.Equals(VerificationCode))
            {
                await _pageService.DisplayAlert("Warning", "Verification Code doesn't match!", "OK");
                return;
            }
            _showVerificationCode = false;
            _showPasswordReset = true;
        }

        private async Task ResetPassword()
        {
            try
            {
                var validation = await _validation.ValidateAsync(this);
                if (validation.IsValid is false)
                {
                    ValidateMessage = validation.GetErrorMesages();
                    return;
                }

                var storedPassword = await SecureStorage.GetAsync(Constant.StoragePasswordKey);
                if (storedPassword.Equals(CurrentPassword) is false)
                {
                    ValidateMessage = "Current Password is invalid.";
                    return;
                }

                await SaveNewPassword();
                await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
            }
            catch (Exception ex)
            { 
                await _pageService.DisplayAlert("Error", ex.Message, "OK");
                throw;
            }
        }

        private async Task SaveNewPassword()
        {
            //TODO: move the secret from the code
            var secret = "KxqP9uJZFaLcIUO0G19XQA==";
            byte[] byteSecret = Encoding.UTF8.GetBytes(secret);
            
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2
            (
                   password: NewPassword,
                   salt: byteSecret,
                   prf: KeyDerivationPrf.HMACSHA256,
                   iterationCount: 100000,
                   numBytesRequested: 256 / 8)
            );
            await SecureStorage.SetAsync(Constant.StoragePasswordKey, hashed);
        }
    }
}
