/* Name: Hao Zhong
 * Course: DVP2
 * Term: April 2021
 * Assignment: 4.1 Beta */

using System;
using System.ComponentModel;
using System.Windows.Input;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace GuessMyName.Models
{
    public class ChatViewModel: INotifyPropertyChanged
    {
        // Fields
        private FirebaseHelper _firebase = new FirebaseHelper();
        private Dictionary<string, LoginViewModel> _answerKeys = new Dictionary<string, LoginViewModel>();

        // Properties
        public string Answer1
        {
            get { return Answer1; }
            set
            {
                Answer1 = value;
                _answerKeys.Add(Answer1, Player1);
            }
        }
        public LoginViewModel Player1 { get; set; } = new LoginViewModel();
        public string Answer2 { get; set; }
        public LoginViewModel Player2
        {
            get { return Player2; }
            set
            {
                Player2 = value;
                _answerKeys.Add(Answer2, Player2);
            }
        }
        public ObservableCollection<MessageModel> Messages { get; set; } = new ObservableCollection<MessageModel>();
        public string TextToSend { get; set; }
        public ICommand OnSendCommand { get; set; }

        // Constructor
        public ChatViewModel()
        {
            OnSendCommand = new Command(() =>
            {
                if(Messages[0].Text.Contains(Answer1))
                {
                    MessagingCenter.Send<LoginViewModel, string>(Player1, "Correct Guess!", Answer1);
                }
                if (Messages[0].Text.Contains(Answer2))
                {
                    MessagingCenter.Send<LoginViewModel, string>(Player2, "Correct Guess!", Answer2);
                }

                SendText();
            });
        }

        async void SendText()
        {
            if (!string.IsNullOrEmpty(TextToSend))
            {
                MessageModel message = new MessageModel() { Text = TextToSend, Sender = MainPage.Me.UserName, Time = DateTime.Now };
                Messages.Insert(0, message);

                await _firebase.SendMessage(TextToSend);

                TextToSend = string.Empty;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
