/* Name: Hao Zhong
 * Course: DVP2
 * Term: April 2021
 * Assignment: 4.1 Beta */

using System;
using System.Collections.Generic;
using GuessMyName.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GuessMyName
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignUpPage : ContentPage
    {
        // Fields
        private List<LoginViewModel> _existingUsers = new List<LoginViewModel>();
        private List<string> _userNames;
        private List<string> _emails;
        private LoginViewModel _vm = new LoginViewModel();
        private FirebaseHelper _firebase = new FirebaseHelper();

        public SignUpPage()
        {
            this.BindingContext = _vm;

            InitializeComponent();

            firstName.Completed += (object sender, EventArgs e) =>
            {
                lastName.Focus();
            };
            lastName.Completed += (object sender, EventArgs e) =>
            {
                userName.Focus();
            };
            userName.Completed += (object sender, EventArgs e) =>
            {
                email.Focus();
            };
            email.Completed += (object sender, EventArgs e) =>
            {
                password.Focus();
            };
            password.Completed += (object sender, EventArgs e) =>
            {
                retypePassword.Focus();
            };
            retypePassword.Completed += (object sender, EventArgs e) =>
            {
                SignUpButton_Clicked(sender, e);
            };

            signUpButton.Clicked += SignUpButton_Clicked;
            loginPageButton.Clicked += LoginPageButton_Clicked;
        }

        private async void SignUpButton_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(firstName.Text) || string.IsNullOrWhiteSpace(lastName.Text) || string.IsNullOrWhiteSpace(userName.Text) || string.IsNullOrWhiteSpace(email.Text) || string.IsNullOrWhiteSpace(password.Text) || string.IsNullOrWhiteSpace(retypePassword.Text))
            {
                await DisplayAlert("Error", "All fields are required, try again", "OK");
                return;
            }

            if (_userNames.Contains(userName.Text))
            {
                await DisplayAlert("UserName already exists", "This username is taken, please choose another one", "OK");
                // Abort, clear the field and return to the sign up screen if any error is encountered.
                userName.Text = string.Empty;
                return;
            }

            if (!email.Text.Contains("@"))
            {
                await DisplayAlert("Error", "No valid email was provided", "OK");
                email.Text = string.Empty;
                return;
            }

            if (_emails.Contains(email.Text))
            {
                await DisplayAlert("Email already exists", "This email is taken, please choose another one", "OK");
                email.Text = string.Empty;
                return;
            }

            if(password.Text != retypePassword.Text)
            {
                await DisplayAlert("The Passwords are not the same", "Password and retyped password must match", "OK");
                password.Text = string.Empty;
                retypePassword.Text = string.Empty;
                return;
            }

            await _firebase.AddUser(firstName.Text, lastName.Text, userName.Text, email.Text, password.Text);

            await DisplayAlert("Success", "Thank you for signing up!", "OK");
            await Navigation.PushModalAsync(new NavigationPage(new MainPage(_vm)));
        }

        private void LoginPageButton_Clicked(object sender, EventArgs e)
        {
            firstName.Text = string.Empty;
            lastName.Text = string.Empty;
            userName.Text = string.Empty;
            email.Text = string.Empty;
            password.Text = string.Empty;
            retypePassword.Text = string.Empty;

            Navigation.PushModalAsync(new LoginPage());
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            // Load existing users
            _existingUsers = await _firebase.GetAllUsers();
            _userNames = new List<string>();
            _emails = new List<string>();

            foreach (LoginViewModel user in _existingUsers)
            {
                _userNames.Add(user.UserName);
                _emails.Add(user.Email);
            }
        }
    }
}
