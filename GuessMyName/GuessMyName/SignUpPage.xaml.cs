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

        public SignUpPage()
        {
            this.BindingContext = _vm;

            InitializeComponent();

            if(File.Exists("../../UserList.txt"))
            {
                using(StreamReader reader = new StreamReader("../../UserList.txt"))
                {
                    while(reader.Peek() > -1)
                    {
                        string[] userProfile = reader.ReadLine().Split('|');
                        _userNames.Add(userProfile[2]);
                        _emails.Add(userProfile[3]);
                    }
                }
            }

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
                SignUpButton_Clicked(sender, e);
            };

            SignUpButton.Clicked += SignUpButton_Clicked;
            LoginPageButton.Clicked += LoginPageButton_Clicked;
        }

        private void SignUpButton_Clicked(object sender, EventArgs e)
        {
            if (_userNames.Contains(UserName.Text))
            {
                DisplayAlert("UserName already exists", "This username is taken, please choose another one", "OK");
                UserName.Text = null;
                return;
            }

            if (!Email.Text.Contains("@"))
            {
                DisplayAlert("Error", "No valid email was provided", "OK");
                Email.Text = null;
                return;
            }

            if (_emails.Contains(Email.Text))
            {
                DisplayAlert("Email already exists", "This email is taken, please choose another one", "OK");
                Email.Text = null;
                return;
            }

            if(Password.Text != RetypePassword.Text)
            {
                DisplayAlert("The Passwords are not the same", "Password and retyped password must match", "OK");
                Password.Text = null;
                RetypePassword.Text = null;
                return;
            }

            if (string.IsNullOrWhiteSpace(FirstName.Text) || string.IsNullOrWhiteSpace(LastName.Text) || string.IsNullOrWhiteSpace(UserName.Text) || string.IsNullOrWhiteSpace(Email.Text) || string.IsNullOrWhiteSpace(Password.Text))
            {
                DisplayAlert("Error", "All fields are required, try again", "OK");
                return;
            }

            using (StreamWriter writer = File.AppendText("../../UserList.txt"))
            {
                writer.WriteLine(FirstName.Text + "|" + LastName.Text + "|" + UserName.Text + "|" + Email.Text + "|" + Password.Text);
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
