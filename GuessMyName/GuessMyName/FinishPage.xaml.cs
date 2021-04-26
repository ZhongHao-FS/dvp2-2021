/* Name: Hao Zhong
 * Course: DVP2
 * Term: April 2021
 * Assignment: 3.1 Alpha */

using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace GuessMyName
{
    public partial class FinishPage : ContentPage
    {
        public FinishPage()
        {
            InitializeComponent();

            mainButton.Clicked += MainButton_Clicked;
            logoutButton.Clicked += LogoutButton_Clicked;
        }

        private void LogoutButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PopToRootAsync();
        }

        private void MainButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
            Navigation.PopAsync();
        }
    }
}
