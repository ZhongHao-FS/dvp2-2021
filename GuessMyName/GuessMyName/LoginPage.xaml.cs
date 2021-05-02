/* Name: Hao Zhong
 * Course: DVP2
 * Term: April 2021
 * Assignment: 4.1 Beta */

using System;
using System.Linq;
using System.Collections.Generic;
using GuessMyName.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GuessMyName
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        // Fields
        private List<LoginViewModel> _existingUsers = new List<LoginViewModel>();
        private Dictionary<string, string> _userPwdPairs;
        private LoginViewModel _vm = new LoginViewModel();
        private FirebaseHelper _firebase = new FirebaseHelper();

        public LoginPage()
        {
            this.BindingContext = _vm;

            InitializeComponent();

            userName.PropertyChanged += UserName_PropertyChanged;
            userName.Completed += (object sender, EventArgs e) =>
            {
                password.Focus();
            };
            password.Completed += (object sender, EventArgs e) =>
            {
                LoginButton_Clicked(sender, e);
            };

            loginButton.Clicked += LoginButton_Clicked;
            signUpPageButton.Clicked += SignUpPageButton_Clicked;

            forgotUserButton.Clicked += ForgotUserButton_Clicked;
            forgotPwdButton.Clicked += ForgotPwdButton_Clicked;
        }

        private void UserName_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            // Check the entered username with stored info in the txt file
            CheckUserProfile();
        }

        private void CheckUserProfile()
        {
            foreach (LoginViewModel user in _existingUsers)
            {
                if (user.UserName == userName.Text)
                {
                    // If the username matches, load all info for this user to get ready to pass _vm to the MainPage.
                    _vm.FirstName = user.FirstName;
                    _vm.LastName = user.LastName;
                    _vm.Email = user.Email;
                }
            }
        }

        private void ForgotPwdButton_Clicked(object sender, EventArgs e)
        {
            DisplayAlert("Hint", $"Password: {_userPwdPairs.First().Value}", "OK");
        }

        private void ForgotUserButton_Clicked(object sender, EventArgs e)
        {
            DisplayAlert("Hint", $"Username: {_userPwdPairs.First().Key}", "OK");
        }

        async void LoginButton_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(userName.Text) || string.IsNullOrWhiteSpace(password.Text))
            {
                await DisplayAlert("Error", "All fields are required, try again", "OK");
                // Abort and return to the login screen if any error is encountered.
                return;
            }

            if (!_userPwdPairs.ContainsKey(userName.Text))
            {
                bool answer = await DisplayAlert("Error", "The user does not exist. Would you like to sign up?", "Sign Up", "Try again");
                if(answer)
                {
                    await Navigation.PopModalAsync();
                }
                
                userName.Text = string.Empty;
                password.Text = string.Empty;
                return;
            }
            else if(_userPwdPairs[userName.Text] != password.Text)
            {
                await DisplayAlert("Error", "Username/password combination is not correct, try again", "OK");
                userName.Text = string.Empty;
                password.Text = string.Empty;
                return;
            }

            _vm.UserName = userName.Text;
            _vm.Password = password.Text;
            await Navigation.PushModalAsync(new NavigationPage(new MainPage(_vm)));
        }

        private void SignUpPageButton_Clicked(object sender, EventArgs e)
        {
            userName.Text = string.Empty;
            password.Text = string.Empty;

            Navigation.PopModalAsync();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            // Load existing users
            _existingUsers = await _firebase.GetAllUsers();
            _userPwdPairs = new Dictionary<string, string>();

            foreach (LoginViewModel user in _existingUsers)
            {
                _userPwdPairs.Add(user.UserName, user.Password);
            }
        }
    }
}
