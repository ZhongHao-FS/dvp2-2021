﻿
using System;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace GuessMyName.Models
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        // Fields
        private string firstName;
        private string lastName;
        private string userName;
        private string email;
        private string password;

        // Properties
        public Action DisplayInvalidLoginPrompt;
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public string FirstName
        {
            get { return firstName; }
            set
            {
                firstName = value;
                PropertyChanged(this, new PropertyChangedEventArgs("FirstName"));
            }
        }
        public string LastName
        {
            get { return lastName; }
            set
            {
                lastName = value;
                PropertyChanged(this, new PropertyChangedEventArgs("LastName"));
            }
        }
        public string UserName
        {
            get { return userName; }
            set
            {
                userName = value;
                PropertyChanged(this, new PropertyChangedEventArgs("UserName"));
            }
        }
        public string Email
        {
            get { return email; }
            set
            {
                email = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Email"));
            }
        }
        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Password"));
            }
        }
        public ICommand SubmitCommand { protected set; get; }

        // Constructor
        public LoginViewModel()
        {
            SubmitCommand = new Command(OnSubmit);
        }

        public void OnSubmit()
        {
            if (userName != "John" || password != "secret")
            {
                DisplayInvalidLoginPrompt();
            }
        }
    }
}
