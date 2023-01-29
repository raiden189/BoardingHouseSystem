using BoardingHouseSystem.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using BoardingHouseSystem.Models;
using System.Threading.Tasks;
using Android.App.Usage;
using Xamarin.Essentials;

namespace BoardingHouseSystem.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public Command LoginCommand { get; }

        public Command SignUpCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new Command(OnLoginClicked);
            SignUpCommand = new Command(OnSignUpClick);
        }

        private async void OnLoginClicked(object obj)
        {
            if (obj != null)
            {
                Login login = obj as Login;
                Login AuthLogin = await App.Database.Authenticate(login);
                if (AuthLogin != null)
                {
                    await App.Current.MainPage.DisplayAlert("Success", "Successfully logged in!", "Ok", FlowDirection.LeftToRight);
                    var account = await this.GetAccount(AuthLogin.Id);
                    if (account.isStudent)
                        await Shell.Current.GoToAsync($"//{nameof(UserPage)}?Id={AuthLogin.Id}");
                    else
                    {
                        Boarding boarding = await App.Database.GetBoardingHouseAsync(AuthLogin.Id);
                        if (boarding != null)
                        {
                            await Shell.Current.GoToAsync($"//{nameof(OwnerPage)}?Id={AuthLogin.Id}");
                        }
                        else
                            await Shell.Current.GoToAsync($"//{nameof(MyBoardingHouse)}?Id={AuthLogin.Id}");
                    }
                }
                else
                    await App.Current.MainPage.DisplayAlert("Error","Incorrect username or password!", "Ok", FlowDirection.LeftToRight);
            }
        }

        private async void OnSignUpClick(object obj)
        {
            await Shell.Current.GoToAsync($"//{nameof(RegisterPage)}");
        }

        private async Task<Account> GetAccount(string id)
        {
            return await App.Database.GetAccountAsync(id);
        }
    }
}
