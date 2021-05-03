/* Name: Hao Zhong
 * Course: DVP2
 * Term: April 2021
 * Assignment: 3.1 Alpha */

using System;
using GuessMyName.Models;
using Xamarin.Forms;

namespace GuessMyName
{
    public partial class FinishPage : ContentPage
    {
        // Field
        private string _correctAnswer;

        public FinishPage(string winner, string answer)
        {
            InitializeComponent();

            _correctAnswer = answer;

            if(winner == MainPage.Me.UserName)
            {
                firstLabel.Text = "Congratulations!";
                secondLabel.Text = "Your guess is correct!";
            }
            else
            {
                firstLabel.Text = "The Game is Over";
                secondLabel.Text = $"{winner} has got the correct answer!";
            }

            mainButton.Clicked += MainButton_Clicked;
            logoutButton.Clicked += LogoutButton_Clicked;
        }

        private void LogoutButton_Clicked(object sender, EventArgs e)
        {
            MainPage.Me = null;
            Navigation.PopToRootAsync();
        }

        private void MainButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
            Navigation.PopAsync();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            WikiAPI wiki = new WikiAPI(_correctAnswer);
            resultImage.Source = ImageSource.FromUri(await wiki.GetImage());
        }
    }
}
