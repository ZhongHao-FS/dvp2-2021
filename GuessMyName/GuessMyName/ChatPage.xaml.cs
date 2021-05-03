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

        public ChatPage(ChatViewModel game, string answerKey)
        {
            InitializeComponent();

            ChatLog = game;
            headerLabel.Text = game.Player1.UserName + "'s Turn";
            _opponentsAnswer = answerKey;
            listView.ItemTapped += ListView_ItemTapped;
            searchButton.Text = $"Search {_opponentsAnswer} on Wikipedia";
            searchButton.Clicked += SearchButton_Clicked;

            if (MainPage.Me != game.Player1)
            {
                NotMyTurn();
            }
            else if (game.Answer1 == string.Empty || game.Player2 == null)
            {
                WaitForPlayer(ChatLog);
            }

            MessagingCenter.Subscribe<LoginViewModel, string>(this, "Correct Guess!", (sender, args) => {
                GameOver(sender.UserName, args);
            });

            MessagingCenter.Subscribe<string>(this, "Listen to new messages", (sender) => {
                if(ChatLog.Player1 == MainPage.Me)
                {
                    headerLabel.Text = ChatLog.Player2.UserName + "'s Turn";
                }
                else
                {
                    headerLabel.Text = ChatLog.Player1.UserName + "'s Turn";
                }

                NotMyTurn();
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
            resultLabel.IsVisible = true;
            searchButton.IsVisible = false;
        }

        async void WaitForPlayer(ChatViewModel openGame)
        {
            ChatLog = await _firebase.ListenNewPlayer(openGame);
        }

        async void GameOver(string winner, string answer)
        {
            await _firebase.DeleteGame(ChatLog);
            _opponentsAnswer = string.Empty;
            ChatLog = null;
            await Navigation.PushModalAsync(new FinishPage(winner, answer));
        }

        async void NotMyTurn()
        {
            MessagingCenter.Send<string>("Not my turn", "My opponent's turn");

            ChatLog.Messages.CollectionChanged += Messages_CollectionChanged;
            ChatLog = await _firebase.ListenNewMessage(ChatLog);
            this.BindingContext = ChatLog;
        }

        private void Messages_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (ChatLog.Messages[0].Text.Contains(ChatLog.Answer1))
            {
                GameOver(ChatLog.Player1.UserName, ChatLog.Answer1);
                return;
            }
            if (ChatLog.Messages[0].Text.Contains(ChatLog.Answer2))
            {
                GameOver(ChatLog.Player2.UserName, ChatLog.Answer2);
                return;
            }

            if (ChatLog.Messages[0].Sender != MainPage.Me.UserName)
            {
                MessagingCenter.Send<string>("My turn", "Show chat buttons");
                headerLabel.Text = MainPage.Me.UserName + "'s Turn";
            }
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            ChatLog = await _firebase.LoadGame();
            this.BindingContext = ChatLog;
        }
    }
}
