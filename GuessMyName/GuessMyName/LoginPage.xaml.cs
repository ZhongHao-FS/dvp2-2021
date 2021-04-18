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
        // Field
        private Dictionary<string, string> _userPwdPairs = new Dictionary<string, string>();
        private LoginViewModel _vm = new LoginViewModel();

        public LoginPage()
        {
            this.BindingContext = _vm;

            InitializeComponent();

            UserName.Completed += (object sender, EventArgs e) =>
            {
                ReadUserProfile();
                Password.Focus();
            };
            Password.Completed += (object sender, EventArgs e) =>
            {
                LoginButton_Clicked(sender, e);
            };

            LoginButton.Clicked += LoginButton_Clicked;
            SignUpPageButton.Clicked += SignUpPageButton_Clicked;

            ForgotUserButton.Clicked += ForgotUserButton_Clicked;
            ForgotPwdButton.Clicked += ForgotPwdButton_Clicked;
        }

        private void ReadUserProfile()
        {
            if (File.Exists("../../UserList.txt"))
            {
                using (StreamReader reader = new StreamReader("../../UserList.txt"))
                {
                    while (reader.Peek() > -1)
                    {
                        string[] userProfile = reader.ReadLine().Split('|');
                        _userPwdPairs.Add(userProfile[2], userProfile[4]);

                        if(userProfile[2] == UserName.Text)
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
            if (string.IsNullOrWhiteSpace(UserName.Text) || string.IsNullOrWhiteSpace(Password.Text))
            {
                await DisplayAlert("Error", "All fields are required, try again", "OK");
                return;
            }

            if (!_userPwdPairs.ContainsKey(UserName.Text))
            {
                bool answer = await DisplayAlert("Error", "The user does not exist. Would you like to sign up?", "Sign Up", "Try again");
                if(answer)
                {
                    await Navigation.PopModalAsync();
                }
                
                UserName.Text = null;
                Password.Text = null;
                return;
            }
            else if(_userPwdPairs[UserName.Text] != Password.Text)
            {
                await DisplayAlert("Error", "Username/password combination is not correct, try again", "OK");
                UserName.Text = null;
                Password.Text = null;
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
