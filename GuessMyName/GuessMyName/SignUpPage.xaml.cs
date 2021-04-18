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
    public partial class SignUpPage : ContentPage
    {
        // Fields
        private List<string> _userNames = new List<string>();
        private List<string> _emails = new List<string>();
        private LoginViewModel _vm = new LoginViewModel();
        private string _filePath = Path.Combine(App.FolderPath, "UserList.txt");

        public SignUpPage()
        {
            this.BindingContext = _vm;

            InitializeComponent();

            // Load existing users
            if(File.Exists(_filePath))
            {
                using(StreamReader reader = new StreamReader(_filePath))
                {
                    while(reader.Peek() > -1)
                    {
                        string[] userProfile = reader.ReadLine().Split('|');
                        _userNames.Add(userProfile[2]);
                        _emails.Add(userProfile[3]);
                    }
                }
            }

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

        private void SignUpButton_Clicked(object sender, EventArgs e)
        {
            if (_userNames.Contains(userName.Text))
            {
                DisplayAlert("UserName already exists", "This username is taken, please choose another one", "OK");
                // Abort, clear the field and return to the sign up screen if any error is encountered.
                userName.Text = null;
                return;
            }

            if (!email.Text.Contains("@"))
            {
                DisplayAlert("Error", "No valid email was provided", "OK");
                email.Text = null;
                return;
            }

            if (_emails.Contains(email.Text))
            {
                DisplayAlert("Email already exists", "This email is taken, please choose another one", "OK");
                email.Text = null;
                return;
            }

            if(password.Text != retypePassword.Text)
            {
                DisplayAlert("The Passwords are not the same", "Password and retyped password must match", "OK");
                password.Text = null;
                retypePassword.Text = null;
                return;
            }

            if (string.IsNullOrWhiteSpace(firstName.Text) || string.IsNullOrWhiteSpace(lastName.Text) || string.IsNullOrWhiteSpace(userName.Text) || string.IsNullOrWhiteSpace(email.Text) || string.IsNullOrWhiteSpace(password.Text) || string.IsNullOrWhiteSpace(retypePassword.Text))
            {
                DisplayAlert("Error", "All fields are required, try again", "OK");
                return;
            }

            if (!File.Exists(_filePath))
            {
                File.Create(_filePath);
            }

            // Always add to the existing user list. The user list is created right before this in case it did not exist earlier.
            using (StreamWriter writer = File.AppendText(_filePath))
            {
                writer.WriteLine(firstName.Text + "|" + lastName.Text + "|" + userName.Text + "|" + email.Text + "|" + password.Text);
            }

            DisplayAlert("Success", "Thank you for signing up!", "OK");
            Navigation.PushModalAsync(new MainPage(_vm));
        }

        private void LoginPageButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new LoginPage());
        }
    }
}
