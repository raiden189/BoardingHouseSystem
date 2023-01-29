using BoardingHouseSystem.Models;
using BoardingHouseSystem.Views;
using Java.Lang;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Xamarin.Forms;

namespace BoardingHouseSystem.ViewModels
{
    public class StudentBoardingViewModel : INotifyPropertyChanged, IQueryAttributable
    {
        public Command SaveCommand { get; set; }

        public Command BackCommand { get; set; }

        public string BookStatus
        {
            get
            { return bookStatus; }
            set
            { bookStatus = value; OnPropertyChanged(); }
        }

        public bool isBookedAlready
        {
            get 
            { return isBooked; }
            set 
            { isBooked = value; OnPropertyChanged(); }
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

        private Boarding boarding;

        private bool isBooked;

        private string bookStatus;

        private string studentId;

        private string boardingId;

        private int boarded = 0;

        private int reservations = 0;

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public StudentBoardingViewModel()
        {
            this.SaveCommand = new Command(OnSave);
            this.BackCommand = new Command(OnBack);
        }

        private async void OnSave(object obj)
        {
            Booking booking = new Booking();
            booking.StudentId = this.studentId;
            booking.BoardingId = this.boardingId;
            booking.Status = System.Enum.GetName(typeof(BOOKSTATUS), BOOKSTATUS.PENDING);
            int bookingSaved = await App.Database.SaveBookingAsync(booking);
            if (bookingSaved > 0)
            {
                await App.Current.MainPage.DisplayAlert("Success", "Account Successfully booked!", "Ok", FlowDirection.LeftToRight);
                this.SetDataBinding();
            }
            else
                await App.Current.MainPage.DisplayAlert("Error", "Error on booking account!", "Ok", FlowDirection.LeftToRight);
        }

        private async void OnBack(object obj)
        {
            ImgPaths = null;
            await Shell.Current.GoToAsync($"//{nameof(UserPage)}?Id={this.studentId}");
        }

        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            if (query.Count > 0)
            {
                this.studentId = HttpUtility.UrlDecode(query["sId"]);
                this.boardingId = HttpUtility.UrlDecode(query["bId"]);
            }
            this.SetDataBinding();
        }

        private async void SetDataBinding()
        {
            this.Boarders = await GetBoardersCount();
            this.Reservations = await GetBoardingRervations();
            this.Boarding = await GetBoarding();
            this.ImgPaths = await GetBoardingImages();
            this.isBookedAlready = await CheckBooking();
        }

        private async Task<Boarding> GetBoarding()
        {
            return await App.Database.GetBoardingHouseAsync(this.boardingId);
        }

        private async Task<List<ImagePath>> GetBoardingImages()
        {
            return await App.Database.GetImagesAsync(this.boardingId);
        }

        public async Task<int> GetBoardersCount()
        {
            var bookings = await App.Database.SearchBookingByStatusOwner(this.boardingId);
            if (bookings.Count() > 0)
                return bookings.Where(w => w.Status == System.Enum.GetName(typeof(BOOKSTATUS), BOOKSTATUS.BOOKED)).Count();
            else
                return 0;
        }

        public async Task<int> GetBoardingRervations()
        {
            var bookings = await App.Database.SearchBookingByStatusOwner(this.boardingId);
            if (bookings.Count() > 0)
                return bookings.Where(w => w.Status == System.Enum.GetName(typeof(BOOKSTATUS), BOOKSTATUS.PENDING)).Count();
            else
                return 0;
        }

        private async Task<bool> CheckBooking()
        {
            var booking = await App.Database.SearchBookingByStatusOwnerStudent(this.studentId, this.boardingId);
            if (booking.Count() > 0)
            {
                this.BookStatus = booking.FirstOrDefault().Status;
                return false;
            }
            else
            {
                var booked = await App.Database.SearchBookingByStatusStudent(this.studentId);
                if (booked.Count() > 0)
                {
                    if (booked.FirstOrDefault().Status == System.Enum.GetName(typeof(BOOKSTATUS), BOOKSTATUS.BOOKED))
                    {
                        this.BookStatus = "Already booked to other boarding house.";
                        return false;
                    }
                    else
                        this.BookStatus = "Not yet booked";
                }
                else
                    this.BookStatus = "Not yet booked";
                return true;
            }
        }
    }
}
