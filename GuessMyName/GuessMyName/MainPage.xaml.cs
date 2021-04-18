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

            searchButton.Clicked += SearchButton_Clicked;
        }

        async void SearchButton_Clicked(object sender, EventArgs e)
        {
            WikiAPI wiki = new WikiAPI(searchEntry.Text);
            resultLabel.Text = await wiki.GetIntro();
        }
    }
}
