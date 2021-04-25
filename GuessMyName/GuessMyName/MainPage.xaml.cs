/* Name: Hao Zhong
 * Course: DVP2
 * Term: April 2021
 * Assignment: 2.1 Sign In/Sign Up */

using System;
using System.IO;
using Xamarin.Forms;
using GuessMyName.Models;

namespace GuessMyName
{
    public partial class MainPage : ContentPage
    {
        // Property
        public static LoginViewModel Player1 { get; set; }
        public static string NameToGuess { get; set; }

        public MainPage(LoginViewModel vm)
        {
            InitializeComponent();

            // User info passed in to display welcome message on the header
            headerLabel.Text = "Welcome, " + vm.UserName;
            Player1 = vm;

            File.Create(Path.Combine(App.FolderPath, "ChatLog.txt"));

            ToolbarItem signOutItem = new ToolbarItem
            {
                Text = "Sign Out",
                Order = ToolbarItemOrder.Primary,
                Priority = 0
            };
            signOutItem.Command = new Command((sender) =>
            {
                this.SignOutCommand();
            });
            this.ToolbarItems.Add(signOutItem);

            promptLabel.Text = "Please enter a person's name for Player2 to guess";
            confirmButton.Clicked += ConfirmButton_Clicked;
        }

        private void ConfirmButton_Clicked(object sender, EventArgs e)
        {
            NameToGuess = nameEntry.Text;

            Navigation.PushModalAsync(new ChatPage());
        }

        private void SignOutCommand()
        {
            Player1 = null;
            NameToGuess = null;

            Navigation.PopModalAsync();
        }
    }
}
