/* Name: Hao Zhong
 * Course: DVP2
 * Term: April 2021
 * Assignment: 2.1 Sign In/Sign Up */

using System;
using System.IO;
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
        private Dictionary<string, string> _userPwdPairs = new Dictionary<string, string>();
        private LoginViewModel _vm = new LoginViewModel();

        public LoginPage()
        {
            this.BindingContext = _vm;

            InitializeComponent();

            userName.Completed += (object sender, EventArgs e) =>
            {
                ReadUserProfile();
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

        private void ReadUserProfile()
        {
            string filePath = Path.Combine(App.FolderPath, "UserList.txt");

            if (File.Exists(filePath))
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    while (reader.Peek() > -1)
                    {
                        string[] userProfile = reader.ReadLine().Split('|');
                        _userPwdPairs.Add(userProfile[2], userProfile[4]);

                        if(userProfile[2] == userName.Text)
                        {
                            _vm.FirstName = userProfile[0];
                            _vm.LastName = userProfile[1];
                            _vm.Email = userProfile[3];
                        }
                    }
                }
            }
        }

        private void ForgotPwdButton_Clicked(object sender, EventArgs e)
        {
            DisplayAlert("Hint", "Password: Secret", "OK");
        }

        private void ForgotUserButton_Clicked(object sender, EventArgs e)
        {
            DisplayAlert("Hint", "Username: John", "OK");
        }

        async void LoginButton_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(userName.Text) || string.IsNullOrWhiteSpace(password.Text))
            {
                await DisplayAlert("Error", "All fields are required, try again", "OK");
                return;
            }

            if (!_userPwdPairs.ContainsKey(userName.Text))
            {
                bool answer = await DisplayAlert("Error", "The user does not exist. Would you like to sign up?", "Sign Up", "Try again");
                if(answer)
                {
                    await Navigation.PopModalAsync();
                }
                
                userName.Text = null;
                password.Text = null;
                return;
            }
            else if(_userPwdPairs[userName.Text] != password.Text)
            {
                await DisplayAlert("Error", "Username/password combination is not correct, try again", "OK");
                userName.Text = null;
                password.Text = null;
                return;
            }

            await Navigation.PushModalAsync(new MainPage(_vm));
        }

        private void SignUpPageButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }
    }
}
