using System;
using System.Collections.Generic;
using System.IO;
using GuessMyName.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GuessMyName
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignUpPage : ContentPage
    {
        public SignUpPage()
        {
            var vm = new LoginViewModel();
            this.BindingContext = vm;
            vm.DisplayInvalidSignUpPrompt += () => DisplayAlert("Error", "All fields are required, try again", "OK");

            InitializeComponent();

            FirstName.Completed += (object sender, EventArgs e) =>
            {
                LastName.Focus();
            };
            LastName.Completed += (object sender, EventArgs e) =>
            {
                UserName.Focus();
            };
            UserName.Completed += (object sender, EventArgs e) =>
            {
                Email.Focus();
            };
            Email.Completed += (object sender, EventArgs e) =>
            {
                Password.Focus();
            };
            Password.Completed += (object sender, EventArgs e) =>
            {
                RetypePassword.Focus();
            };
            RetypePassword.Completed += (object sender, EventArgs e) =>
            {
                vm.SignUpCommand.Execute(null);
            };

            LoginPageButton.Clicked += LoginPageButton_Clicked;
        }

        private void LoginPageButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new LoginPage());
        }

        public static void RecordNewUser()
        {

        }
    }
}
