/* Name: Hao Zhong
 * Course: DVP2
 * Term: April 2021
 * Assignment: 3.1 Alpha */

using System;
using System.Collections.Generic;
using GuessMyName.Models;
using Xamarin.Forms;

namespace GuessMyName
{
    public partial class ChatPage : ContentPage
    {
        public ChatPage()
        {
            InitializeComponent();
            this.BindingContext = new ChatViewModel();

            listView.ItemTapped += ListView_ItemTapped;
            searchButton.Clicked += SearchButton_Clicked;
        }

        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            chatInput.UnFocusEntry();
        }

        async void SearchButton_Clicked(object sender, EventArgs e)
        {
            WikiAPI wiki = new WikiAPI(searchEntry.Text);
            resultLabel.Text = await wiki.GetIntro();
        }
    }
}
