/* Name: Hao Zhong
 * Course: DVP2
 * Term: April 2021
 * Assignment: 3.1 Alpha */

using System;
using GuessMyName.Models;
using Xamarin.Forms;

namespace GuessMyName
{
    public partial class ChatPage : ContentPage
    {
        // Fields
        private FirebaseHelper _firebase = new FirebaseHelper();
        private string _opponentsAnswer;

        public ChatPage(string answerKey)
        {
            InitializeComponent();

            _opponentsAnswer = answerKey;
            listView.ItemTapped += ListView_ItemTapped;
            searchButton.Text = $"Search {_opponentsAnswer} on Wikipedia";
            searchButton.Clicked += SearchButton_Clicked;

            MessagingCenter.Subscribe<LoginViewModel, string>(this, "Correct Guess!", (sender, args) => {
                Navigation.PushModalAsync(new FinishPage(sender.UserName, args));
            });
        }

        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            chatInput.UnFocusEntry();
        }

        async void SearchButton_Clicked(object sender, EventArgs e)
        {
            WikiAPI wiki = new WikiAPI(_opponentsAnswer);
            resultLabel.Text = await wiki.GetIntro();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            this.BindingContext = _firebase.LoadGame();
        }
    }
}
