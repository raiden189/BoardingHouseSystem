using BoardingHouseSystem.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Xamarin.Forms;
using System.Net.Http.Headers;
using Xamarin.Essentials;
using System.Web;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using BoardingHouseSystem.Views;
using BoardingHouseSystem.Data;

namespace BoardingHouseSystem.ViewModels
{
    public class OwnerPageViewModel : INotifyPropertyChanged, IQueryAttributable
    {
        public Command LoadMapCommand { get; }

        public Command ApplyCommand { get;  }

        public Command DenyCommand { get; }

        public Command BackCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public int Boarders
        {
            get
            { return boarded; }
            set
            { boarded = value; OnPropertyChanged(); }
        }

        public int Reservations
        {
            get
            { return reservations; }
            set
            { reservations = value; OnPropertyChanged(); }
        }

        public List<Account> Students
        {
            get
            { return students; }
            set
            { students = value; OnPropertyChanged(); }
        }

        public List<ImagePath> ImgPaths
        {
            get
            { return imgPaths; }
            set
            { imgPaths = value; OnPropertyChanged(); }
        }

        public Boarding Boarding
        {
            get
            { return boarding; }
            set
            { boarding = value; OnPropertyChanged(); }
        }

        private List<ImagePath> imgPaths;

        private List<Account> students;

        private Boarding boarding;

        private string Id;

        private int boarded = 0;

        private int reservations = 0;

        public OwnerPageViewModel()
        {
            LoadMapCommand = new Command(LoadMap);
            BackCommand = new Command(OnBack);
            ApplyCommand = new Command(OnApply);
            DenyCommand = new Command(OnDeny);
        }

        private async void OnApply(object obj)
        {
            Account student = (Account)obj;
            var bookings = await App.Database.SearchBookingByStatusOwnerStudent(student.Id, this.Id);
            if (bookings.Count() > 0)
            {
                Booking booking = bookings.FirstOrDefault();
                booking.Status = System.Enum.GetName(typeof(BOOKSTATUS), BOOKSTATUS.BOOKED);
                int saveBooking = await App.Database.UpdateBookingAsync(booking);
                if (saveBooking > 0)
                {
                    await App.Current.MainPage.DisplayAlert("Success", "Student successfully booked", "Ok", FlowDirection.LeftToRight);
                    this.Students.Remove(student);
                }
                else
                    await App.Current.MainPage.DisplayAlert("Error", "Error on booking student", "Ok", FlowDirection.LeftToRight);
            }
            else
                await App.Current.MainPage.DisplayAlert("Error", "Unable to find transaction", "Ok", FlowDirection.LeftToRight);
            this.SetBinding();

        }

        private async void OnDeny(object obj)
        {
            Account student = (Account)obj;
            var bookings = await App.Database.SearchBookingByStatusOwnerStudent(student.Id, this.Id);
            if (bookings.Count() > 0)
            {
                Booking booking = bookings.FirstOrDefault();
                booking.Status = System.Enum.GetName(typeof(BOOKSTATUS), BOOKSTATUS.DENIED);
                int saveBooking = await App.Database.UpdateBookingAsync(booking);
                if (saveBooking > 0)
                {
                    await App.Current.MainPage.DisplayAlert("Success", "Student booking successfully updated", "Ok", FlowDirection.LeftToRight);
                    this.Students.Remove(student);
                }
                else
                    await App.Current.MainPage.DisplayAlert("Error", "Error on booking student", "Ok", FlowDirection.LeftToRight);
            }
            else
                await App.Current.MainPage.DisplayAlert("Error", "Unable to find transaction", "Ok", FlowDirection.LeftToRight);
            this.SetBinding();
        }

        public async void LoadMap(object obj)
        {
            if(Device.RuntimePlatform.Equals(Device.iOS))
                await Launcher.OpenAsync("https://www.google.com/maps/dir/Jose+Rizal+Memorial+State+University+-+Main+Campus,+MC4F%2B936,+Gov.+Sta.+Cruz,+Guading+Adasa+St,+Dapitan+City,+Zamboanga+del+Norte/Don+Hospicio+Ochhtorena+St,+Dapitan+City,+Zamboanga+del+Norte/@8.6571671,123.4202828,17z/data=!3m1!4b1!4m13!4m12!1m5!1m1!1s0x3254bf4454a8bf07:0xfc69ea2ff39b5bfd!2m2!1d123.422625!2d8.6559176!1m5!1m1!1s0x3254bf4f3badc66f:0x4c283a3a8b8b2c78!2m2!1d123.4224366!2d8.6583828");
            else if(Device.RuntimePlatform.Equals(Device.Android))
                await Launcher.OpenAsync("https://www.google.com/maps/dir/Jose+Rizal+Memorial+State+University+-+Main+Campus,+MC4F%2B936,+Gov.+Sta.+Cruz,+Guading+Adasa+St,+Dapitan+City,+Zamboanga+del+Norte/Don+Hospicio+Ochhtorena+St,+Dapitan+City,+Zamboanga+del+Norte/@8.6571671,123.4202828,17z/data=!3m1!4b1!4m13!4m12!1m5!1m1!1s0x3254bf4454a8bf07:0xfc69ea2ff39b5bfd!2m2!1d123.422625!2d8.6559176!1m5!1m1!1s0x3254bf4f3badc66f:0x4c283a3a8b8b2c78!2m2!1d123.4224366!2d8.6583828");
        }

        private async void OnBack(object obj)
        {
            ImgPaths = null;
            var account = await this.GetAccount(this.Id);
            if(account.isStudent)
                await Shell.Current.GoToAsync($"//{nameof(StudentProfilePage)}?Id={this.Id}");
            else
                Application.Current.MainPage = new AppShell();
        }

        private async Task<List<Account>> GetStudents()
        {
            List<Account> studentAccounts = new List<Account>();
            var bookings = await App.Database.SearchBookingByStatusOwner(this.Id, System.Enum.GetName(typeof(BOOKSTATUS), BOOKSTATUS.PENDING));
            if (bookings != null)
            {
                foreach (Booking booking in bookings)
                {
                    Account student = await GetAccount(booking.StudentId);
                    studentAccounts.Add(student);
                }
            }
            return studentAccounts;
        }

        private async Task<Boarding> GetBoarding()
        {
            return await App.Database.GetBoardingHouseAsync(this.Id);
        }

        private async Task<List<ImagePath>> GetImages()
        {
            return await App.Database.GetImagesAsync(this.Id);
        }

        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            if (query.Count > 0)
                this.Id = HttpUtility.UrlDecode(query["Id"]);
            this.SetBinding();
        }

        private async void SetBinding()
        {
            this.Boarders = await GetBoardersCount();
            this.Reservations = await GetBoardingRervations();
            this.ImgPaths = await GetImages();
            this.Boarding = await GetBoarding();
            this.Students = await GetStudents();
        }
        public async Task<int> GetBoardersCount()
        {
            var bookings = await App.Database.SearchBookingByStatusOwner(this.Id);
            if (bookings.Count() > 0)
                return bookings.Where(w => w.Status == System.Enum.GetName(typeof(BOOKSTATUS), BOOKSTATUS.BOOKED)).Count();
            else
                return 0;
        }

        public async Task<int> GetBoardingRervations()
        {
            var bookings = await App.Database.SearchBookingByStatusOwner(this.Id);
            if (bookings.Count() > 0)
                return bookings.Where(w => w.Status == System.Enum.GetName(typeof(BOOKSTATUS), BOOKSTATUS.PENDING)).Count();
            else
                return 0;
        }


        private async Task<Account> GetAccount(string id)
        {
            return await App.Database.GetAccountAsync(id);
        }
    }
}
