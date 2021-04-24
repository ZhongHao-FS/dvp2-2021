/* Name: Hao Zhong
 * Course: DVP2
 * Term: April 2021
 * Assignment: 3.1 Alpha */

using System;
using System.ComponentModel;
using System.Windows.Input;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace GuessMyName.Models
{
    public class ChatViewModel: INotifyPropertyChanged
    {
        // Properties
        public ObservableCollection<MessageModel> Messages { get; set; } = new ObservableCollection<MessageModel>();
        public string TextToSend { get; set; }
        public ICommand OnSendCommand { get; set; }

        // Constructor
        public ChatViewModel()
        {
            OnSendCommand = new Command(() =>
            {
                if (!string.IsNullOrEmpty(TextToSend))
                {
                    Messages.Insert(0, new MessageModel() { Text = TextToSend, Sender = MainPage.Player1.UserName });
                    TextToSend = string.Empty;
                }
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
