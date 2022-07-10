﻿using System;
using ep.Mobile.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ep.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"//{nameof(RegisterPage)}");
        }

        private async void TapGestureRecognizer_ResetPassword(object sender, EventArgs e)
        {
            await Shell.Current.Navigation.PushAsync(new ResetPassword());
        }     
    }
}