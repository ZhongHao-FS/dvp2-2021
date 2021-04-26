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
            searchButton.Text = $"Search {MainPage.NameToGuess} on Wikipedia";
            searchButton.Clicked += SearchButton_Clicked;

            MessagingCenter.Subscribe<string>(this, "Correct Guess!", (sender) => {
                Navigation.PushModalAsync(new FinishPage());
            });
        }

        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            chatInput.UnFocusEntry();
        }

        async void SearchButton_Clicked(object sender, EventArgs e)
        {
            WikiAPI wiki = new WikiAPI(MainPage.NameToGuess);
            resultLabel.Text = await wiki.GetIntro();
        }
    }
}
