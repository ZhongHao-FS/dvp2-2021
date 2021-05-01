/* Name: Hao Zhong
 * Course: DVP2
 * Term: April 2021
 * Assignment: 3.1 Alpha */

using System;
using GuessMyName.Models;
using Xamarin.Forms;

namespace GuessMyName.Views
{
    public partial class ChatInputBarView : ContentView
    {
        public ChatInputBarView()
        {
            InitializeComponent();
        }

        public void Handle_Completed(object sender, EventArgs e)
        {
            (this.Parent.Parent.BindingContext as ChatViewModel).OnSendCommand.Execute(null);
            chatTextInput.Focus();
        }

        public void UnFocusEntry()
        {
            chatTextInput?.Unfocus();
        }
    }
}
