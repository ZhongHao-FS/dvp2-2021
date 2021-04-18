/* Name: Hao Zhong
 * Course: DVP2
 * Term: April 2021
 * Assignment: 2.1 Sign In/Sign Up */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using GuessMyName.Models;

namespace GuessMyName
{
    public partial class MainPage : ContentPage
    {
        public MainPage(LoginViewModel vm)
        {
            InitializeComponent();

            headerLabel.Text = "Welcome, " + vm.UserName;

            searchButton.Clicked += SearchButton_Clicked;
        }

        async void SearchButton_Clicked(object sender, EventArgs e)
        {
            WikiAPI wiki = new WikiAPI(searchEntry.Text);
            resultLabel.Text = await wiki.GetIntro();
        }
    }
}
