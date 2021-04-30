/* Name: Hao Zhong
 * Course: DVP2
 * Term: April 2021
 * Assignment: 4.1 Beta */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;

namespace GuessMyName.Models
{
    public class FirebaseHelper
    {
        // Field
        private FirebaseClient _firebase = new FirebaseClient("https://guesswho-5ac31-default-rtdb.firebaseio.com/");

        public async Task<List<LoginViewModel>> GetAllUsers()
        {
            return (await _firebase.Child("LoginViewModel").OnceAsync<LoginViewModel>()).Select(item => new LoginViewModel
              {
                  FirstName = item.Object.FirstName,
                  LastName = item.Object.LastName,
                  UserName = item.Object.UserName,
                  Email = item.Object.Email,
                  Password = item.Object.Password
            }).ToList();
        }

        public async Task AddUser(string first, string last, string userName, string email, string pwd)
        {
            await _firebase.Child("LoginViewModel").PostAsync(new LoginViewModel()
            {
                FirstName = first,
                LastName = last,
                UserName = userName,
                Email = email,
                Password = pwd
            });
        }

        public async Task NewGame(LoginViewModel player1, string answer2)
        {
            await _firebase.Child("ChatViewModel").PostAsync(new ChatViewModel()
            {
                Answer1 = string.Empty,
                Player1 = player1,
                Answer2 = answer2,
                Player2 = null
            });
        }

        public async Task JoinGame(LoginViewModel player2, string answer1)
        {
            var gameToJoin = (await _firebase.Child("ChatViewModel").OnceAsync<ChatViewModel>())
                .Where(a => string.IsNullOrEmpty(a.Object.Answer1)).FirstOrDefault();

            gameToJoin.Object.Answer1 = answer1;
            gameToJoin.Object.Player2 = player2;

            await _firebase.Child("ChatViewModel").Child(gameToJoin.Key).PutAsync(gameToJoin);
        }

        public async Task<ChatViewModel> LoadGame()
        {
            var allGames = (await _firebase.Child("ChatViewModel").OnceAsync<ChatViewModel>()).Select(item => new ChatViewModel
            {
                Player1 = item.Object.Player1,
                Answer1 = item.Object.Answer1,
                Answer2 = item.Object.Answer2,
                Player2 = item.Object.Player2,
                Messages = item.Object.Messages
            }).ToList();
            await _firebase.Child("ChatViewModel").OnceAsync<ChatViewModel>();

            return allGames.Where(a => a.Player1 == MainPage.Me || a.Player2 == MainPage.Me).FirstOrDefault();
        }
    }
}
