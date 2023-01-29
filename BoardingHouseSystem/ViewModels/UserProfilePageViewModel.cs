using BoardingHouseSystem.Models;
using BoardingHouseSystem.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Xamarin.Forms;

namespace BoardingHouseSystem.ViewModels
{
    public class UserProfilePageViewModel : IQueryAttributable, INotifyPropertyChanged
    {
        public Command BackCommand { get; }

        public Command LogoutCommand { get; }

        public Command ViewBoardingHouse { get; set; }

        public Account Account 
        {
            get 
            { return account; }
            set 
            {
                account = value;
                OnPropertyChanged();
            }
        }

        public string AccountType 
        {
            get 
            { return accountType; } 
            set
            { accountType = value; OnPropertyChanged(); }
        }

        public string AccountFullName
        {
            get
            { return accountFullName; }
            set
            { accountFullName = value; OnPropertyChanged(); }
        }

        private string Id;

        private Account account;

        private string accountType;

        private string accountFullName;

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public UserProfilePageViewModel()
        {
            LogoutCommand = new Command(OnLogoutCommand);
            BackCommand = new Command(OnBackCommand);
            ViewBoardingHouse = new Command(OnAddBoardingHouse);
        }

        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            if (query.Count > 0)
                this.Id = HttpUtility.UrlDecode(query["Id"]);
            this.SetAccountBinding();
        }

        private async void OnAddBoardingHouse(object obj)
        {
            if (this.account.isStudent)
                await Shell.Current.GoToAsync($"//{nameof(StudentBoardingView)}?Id={this.Id}");
            else
            {
                Boarding boarding = await App.Database.GetBoardingHouseAsync(this.Id);
                if (boarding != null)
                    await Shell.Current.GoToAsync($"//{nameof(OwnerPage)}?Id={this.Id}");
                else
                    await Shell.Current.GoToAsync($"//{nameof(MyBoardingHouse)}?Id={this.Id}");
            }
        }

        private async void OnBackCommand(object obj)
        {
            await Shell.Current.GoToAsync($"//{nameof(UserPage)}?Id={Id}");
        }

        private async void OnLogoutCommand(object obj)
        {
            Application.Current.MainPage = new AppShell();
        }

        private async void SetAccountBinding()
        {
            this.Account = await GetAccount();
            this.AccountType = this.account.isStudent ? "Student" : "Owner";
            this.AccountFullName = string.Concat(this.account.FirstName.ToUpper(), " ", this.account.MiddleName.ToUpper(), " ", this.account.LastName.ToUpper()).Replace("%20", " ");
        }

        private async Task<Account> GetAccount()
        {
            return await App.Database.GetAccountAsync(this.Id);
        }
    }
}
