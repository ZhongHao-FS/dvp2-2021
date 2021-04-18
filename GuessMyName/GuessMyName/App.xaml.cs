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

            // FolderPath is generated for the streamreader and writer functions. For access reasons the final filepath has to be combined on each page before use.
            FolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));

            // As per assignment instruction, the SignUpPage should be the first page shown after loading the app.
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
