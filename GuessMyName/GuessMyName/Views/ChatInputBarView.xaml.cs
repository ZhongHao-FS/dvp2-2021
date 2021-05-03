/* Name: Hao Zhong
 * Course: DVP2
 * Term: April 2021
 * Assignment: 4.1 Beta */

using System;
using Xamarin.Forms;

namespace GuessMyName.Views
{
    public partial class ChatInputBarView : ContentView
    {
        public ChatInputBarView()
        {
            InitializeComponent();

            this.BindingContext = ChatPage.ChatLog;

            MessagingCenter.Subscribe<string>(this, "My opponent's turn", (sender) => {
                chatTextInput.IsVisible = true;
                chatTextInput.IsEnabled = false;
                sendLabel.IsVisible = true;
                sendLabel.IsEnabled = false;
                yesButton.IsVisible = false;
                noButton.IsVisible = false;
            });

            MessagingCenter.Subscribe<string>(this, "Shot chat buttons", (sender) => {
                chatTextInput.IsVisible = false;
                chatTextInput.IsEnabled = false;
                sendLabel.IsVisible = false;
                sendLabel.IsEnabled = false;
                yesButton.IsVisible = true;
                noButton.IsVisible = true;
            });

            yesButton.Clicked += YesButton_Clicked;
            noButton.Clicked += NoButton_Clicked;
        }

        private void NoButton_Clicked(object sender, EventArgs e)
        {
            ChatPage.ChatLog.TextToSend = "No";
            ChatPage.ChatLog.OnSendCommand.Execute(null);

            chatTextInput.IsVisible = true;
            chatTextInput.IsEnabled = true;
            sendLabel.IsVisible = true;
            sendLabel.IsEnabled = true;
            yesButton.IsVisible = false;
            noButton.IsVisible = false;
        }

        private void YesButton_Clicked(object sender, EventArgs e)
        {
            ChatPage.ChatLog.TextToSend = "Yes";
            ChatPage.ChatLog.OnSendCommand.Execute(null);

            chatTextInput.IsVisible = true;
            chatTextInput.IsEnabled = true;
            sendLabel.IsVisible = true;
            sendLabel.IsEnabled = true;
            yesButton.IsVisible = false;
            noButton.IsVisible = false;
        }

        public void Handle_Completed(object sender, EventArgs e)
        {
            ChatPage.ChatLog.OnSendCommand.Execute(null);

            MessagingCenter.Send<string>("End my turn", "Listen to new messages");
        }

        public void UnFocusEntry()
        {
            chatTextInput?.Unfocus();
        }
    }
}
