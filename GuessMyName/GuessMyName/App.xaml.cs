/* Name: Hao Zhong
 * Course: DVP2
 * Term: April 2021
 * Assignment: 2.1 Sign In/Sign Up */

using System;
using System.IO;
using Xamarin.Forms;

namespace GuessMyName
{
    public partial class App : Application
    {
        // Property
        public static string FolderPath { get; private set; }

        public App()
        {
            InitializeComponent();

            FolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));

            MainPage = new SignUpPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
