/* Name: Hao Zhong
 * Course: DVP2
 * Term: April 2021
 * Assignment: 3.1 Alpha */

using System;
using System.IO;
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
            string filePath = Path.Combine(App.FolderPath, "ChatLog.txt");
            using (StreamReader reader = new StreamReader(filePath))
            {
                while (reader.Peek() > -1)
                {
                    string[] chatLog = reader.ReadLine().Split('|');
                    MessageModel chat = new MessageModel() { Text = chatLog[0], Sender = chatLog[1], Time = DateTime.Parse(chatLog[2]) };
                    Messages.Insert(0, chat);
                }
            }

            OnSendCommand = new Command(() =>
            {
                if (!string.IsNullOrEmpty(TextToSend))
                {
                    MessageModel message = new MessageModel() { Text = TextToSend, Sender = MainPage.Player1.UserName, Time = DateTime.Now };
                    Messages.Insert(0, message);

                    using (StreamWriter writer = File.AppendText(filePath))
                    {
                        writer.WriteLine(message.Text + "|" + message.Sender + "|" + message.Time);
                    }

                    TextToSend = string.Empty;
                }
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
