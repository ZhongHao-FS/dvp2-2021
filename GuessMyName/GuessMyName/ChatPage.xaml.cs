/* Name: Hao Zhong
 * Course: DVP2
 * Term: April 2021
 * Assignment: 4.1 Beta */

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

        // Property
        public static ChatViewModel ChatLog { get; set; } = new ChatViewModel();

        public ChatPage(string answerKey)
        {
            InitializeComponent();

            headerLabel.Text = ChatLog.Player1.UserName + "'s Turn";
            _opponentsAnswer = answerKey;
            listView.ItemTapped += ListView_ItemTapped;
            searchButton.Text = $"Search {_opponentsAnswer} on Wikipedia";
            searchButton.Clicked += SearchButton_Clicked;

            MessagingCenter.Subscribe<LoginViewModel, string>(this, "Correct Guess!", (sender, args) => {
                Navigation.PushModalAsync(new FinishPage(sender.UserName, args));
            });

            MessagingCenter.Subscribe<string>(this, "Listen to new messages", (sender) => {
                this.BindingContext = _firebase.ListenNewMessage(ChatLog);
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

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            ChatLog = await _firebase.LoadGame();
            this.BindingContext = ChatLog;

            if(ChatLog.Answer1 == string.Empty || ChatLog.Player2 == null)
            {
                ChatLog = await _firebase.ListenNewPlayer(ChatLog);
            }
        }
    }
}
