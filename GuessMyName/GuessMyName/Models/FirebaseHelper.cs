/* Name: Hao Zhong
 * Course: DVP2
 * Term: April 2021
 * Assignment: 4.1 Beta */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
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

        public async Task<bool> FindOpenGame(LoginViewModel me)
        {
            var openGames = (await _firebase.Child("ChatViewModel").OnceAsync<ChatViewModel>())
                .Where(a => string.IsNullOrEmpty(a.Object.Answer1) && a.Object.Player1 != me).ToList();

            if(openGames.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task JoinGame(LoginViewModel player2, string answer1)
        {
            var gameToJoin = (await _firebase.Child("ChatViewModel").OnceAsync<ChatViewModel>())
                .Where(a => string.IsNullOrEmpty(a.Object.Answer1) && a.Object.Player1 != player2).FirstOrDefault();
            
            gameToJoin.Object.Answer1 = answer1;
            gameToJoin.Object.Player2 = player2;

            string key = (await _firebase.Child("ChatViewModel").OnceAsync<ChatViewModel>())
                .Where(a => string.IsNullOrEmpty(a.Object.Answer1) && a.Object.Player1 != player2).FirstOrDefault().Key;

            await _firebase.Child("ChatViewModel").Child(key).PutAsync(gameToJoin);
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

            return allGames.Where(a => a.Player1 == MainPage.Me || a.Player2 == MainPage.Me).FirstOrDefault();
        }

        public async Task<ChatViewModel> ListenNewPlayer(ChatViewModel myGame)
        {
            var gameToListen = (await _firebase.Child("ChatViewModel").OnceAsync<ChatViewModel>())
                .Where(a => a.Object == myGame).FirstOrDefault();

            while(gameToListen.Object == myGame)
            {
                gameToListen = (await _firebase.Child("ChatViewModel").OnceAsync<ChatViewModel>())
                .Where(a => a.Object.Player1 == myGame.Player1 && a.Object.Answer2 == myGame.Answer2).FirstOrDefault();
            }

            return gameToListen.Object;
        }

        public async Task SendMessage(string text)
        {
            var gameToSend = (await _firebase.Child("ChatViewModel").OnceAsync<ChatViewModel>())
                .Where(a => a.Object.Player1 == MainPage.Me || a.Object.Player2 == MainPage.Me).FirstOrDefault();

            gameToSend.Object.Messages.Add(new MessageModel() { Text = text, Sender = MainPage.Me.UserName, Time = DateTime.Now });

            string key = (await _firebase.Child("ChatViewModel").OnceAsync<ChatViewModel>())
                .Where(a => a.Object.Player1 == MainPage.Me || a.Object.Player2 == MainPage.Me).FirstOrDefault().Key;

            await _firebase.Child("ChatViewModel").Child(key).Child("Messages").PutAsync(gameToSend.Object.Messages);
        }

        public async Task<ChatViewModel> ListenNewMessage(ChatViewModel chat)
        {
            var gameToListen = (await _firebase.Child("ChatViewModel").OnceAsync<ChatViewModel>())
                .Where(a => a.Object.Player1 == chat.Player1 && a.Object.Player2 == chat.Player2).FirstOrDefault();

            while(gameToListen.Object.Messages == chat.Messages)
            {
                gameToListen = (await _firebase.Child("ChatViewModel").OnceAsync<ChatViewModel>())
                .Where(a => a.Object.Player1 == chat.Player1 && a.Object.Player2 == chat.Player2).FirstOrDefault();
            }

            return gameToListen.Object;
        }

        public async Task DeleteGame(ChatViewModel game)
        {
            string key = (await _firebase.Child("ChatViewModel").OnceAsync<ChatViewModel>())
                .Where(a => a.Object == game).FirstOrDefault().Key;
            await _firebase.Child("ChatViewModel").Child(key).DeleteAsync();
        }
    }
}
