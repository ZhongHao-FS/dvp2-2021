using System;
using System.Collections.Generic;
using GuessMyName.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GuessMyName
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            var vm = new LoginViewModel();
            this.BindingContext = vm;
            vm.DisplayInvalidLoginPrompt += () => DisplayAlert("Error", "Invalid Login, try again", "OK");

            InitializeComponent();

            UserName.Completed += (object sender, EventArgs e) =>
            {
                Password.Focus();
            };
            Password.Completed += (object sender, EventArgs e) =>
            {
                vm.LoginCommand.Execute(null);
            };

            SignUpPageButton.Clicked += SignUpPageButton_Clicked;
        }

        private void SignUpPageButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }
    }
}
