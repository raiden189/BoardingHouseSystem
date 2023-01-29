using BoardingHouseSystem.Views;
using System.Collections.Generic;
using System.Web;
using Xamarin.Forms;
using BoardingHouseSystem.Models;
using BoardingHouseSystem.Services;
using Newtonsoft.Json;
using Java.Lang;
using System;
using BoardingHouseSystem.Data;
using System.Linq;
using System.Runtime.InteropServices;

namespace BoardingHouseSystem.ViewModels
{
    public class RegisterUserViewModel : IQueryAttributable
    {
        public Command SaveCommand { get; set; }

        public Command BackCommand { get; set; }

        private Account account;

        public RegisterUserViewModel()
        {
            SaveCommand = new Command(OnRegisterClicked);
            BackCommand = new Command(OnBackClicked);
        }

        private async void OnRegisterClicked(object obj)
        {
            Login login = obj as Login;
            Validate(login);
            string Id = Guid.NewGuid().ToString("N");
            string dlLink = await FireBaseApi.UploadFile(FileHandler.GenerateNewFileName(this.account.Photo), this.account.Photo);
            if (dlLink != string.Empty)
            {
                FileHandler.DisposeFile(this.account.Photo);
                this.account.Id = Id;
                this.account.Photo = dlLink;
                int i = await App.Database.SaveAccountAsync(this.account);
                if (i > 0)
                {
                    login.Id = Id;
                    await App.Database.SaveLoginAsync(login);
                    await Shell.Current.DisplayAlert("Success", "Account successfully saved.", "OK");
                    Application.Current.MainPage = new AppShell();
                }
                else
                    await Shell.Current.DisplayAlert("Error", "Error on saving account.", "OK");
            }
            else
                await Shell.Current.DisplayAlert("Error", "Error on uploading file.", "OK");
        }

        private async void OnBackClicked(object obj)
        {
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            await Shell.Current.GoToAsync($"//{nameof(RegisterPage)}");
        }

        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            if (query.Count > 0)
            {
                string jsonAccount = query["account"].Replace("%7B", "{").Replace("%7D", "}").Replace("%22", "\"");
                this.account = System.Text.Json.JsonSerializer.Deserialize<Account>(jsonAccount);
            }
        }

        private async void Validate(Login loginObject)
        {
            Login login = await App.Database.CheckUserName(loginObject.Username);
            if (login != null)
            {
                _ = Shell.Current.DisplayAlert("Error", "Username already exists.", "OK");
                return;
            }
        }
    }
}
