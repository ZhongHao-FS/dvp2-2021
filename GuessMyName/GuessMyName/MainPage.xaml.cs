/* Name: Hao Zhong
 * Course: DVP2
 * Term: April 2021
 * Assignment: 2.1 Sign In/Sign Up */

using System;
using Xamarin.Forms;
using GuessMyName.Models;

namespace GuessMyName
{
    public partial class MainPage : ContentPage
    {
        // Field
        FirebaseHelper _firebase = new FirebaseHelper();

        // Property
        public static LoginViewModel Me { get; set; } = new LoginViewModel();

        public MainPage(LoginViewModel vm)
        {
            InitializeComponent();

            // User info passed in to display welcome message on the header
            headerLabel.Text = "Welcome, " + vm.UserName;
            Me = vm;

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
            switchButton.Text = "Join a Game";
            confirmButton.Clicked += ConfirmButton_Clicked;
            switchButton.Clicked += SwitchButton_Clicked;
        }

        async void SwitchButton_Clicked(object sender, EventArgs e)
        {
            promptLabel.Text = "Please enter a person's name for Player1 to guess";
            switchButton.Text = "New Game";

            if(! await _firebase.FindOpenGame(Me))
            {
                await DisplayAlert("Oops", "Currently there is no game to join!", "Back to Create New Game");
                SwitchBack_Clicked(sender, e);
            }

            confirmButton.Clicked += JoinButton_Clicked;
            switchButton.Clicked += SwitchBack_Clicked;
        }

        private void SwitchBack_Clicked(object sender, EventArgs e)
        {
            promptLabel.Text = "Please enter a person's name for Player2 to guess";
            switchButton.Text = "Join a Game";
            confirmButton.Clicked += ConfirmButton_Clicked;
            switchButton.Clicked += SwitchButton_Clicked;
        }

        async void ConfirmButton_Clicked(object sender, EventArgs e)
        {
            await _firebase.NewGame(Me, nameEntry.Text);

            await Navigation.PushAsync(new ChatPage(nameEntry.Text));
        }

        async void JoinButton_Clicked(object sender, EventArgs e)
        {
            await _firebase.JoinGame(Me, nameEntry.Text);

            await Navigation.PushAsync(new ChatPage(nameEntry.Text));
        }

        private void SignOutCommand()
        {
            Me = null;
            nameEntry.Text = string.Empty;

            Navigation.PopModalAsync();
        }
    }
}
