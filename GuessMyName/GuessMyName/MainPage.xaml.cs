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
        // Property
        public static LoginViewModel Player1 { get; set; }

        public MainPage(LoginViewModel vm)
        {
            InitializeComponent();

            // User info passed in to display welcome message on the header
            headerLabel.Text = "Welcome, " + vm.UserName;
            Player1 = vm;

            searchButton.Clicked += SearchButton_Clicked;
        }

        private void SearchButton_Clicked(object sender, EventArgs e)
        {
            
        }
    }
}
