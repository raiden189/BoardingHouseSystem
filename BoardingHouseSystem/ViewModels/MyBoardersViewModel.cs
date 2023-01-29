using BoardingHouseSystem.Models;
using Java.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Xamarin.Forms;
using static Android.Provider.MediaStore;

namespace BoardingHouseSystem.ViewModels
{
    public class MyBoardersViewModel : INotifyPropertyChanged, IQueryAttributable
    {

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Command ViewProfile { get; }

        public Command RemoveCommand { get; }

        public Command SelectedChange { get; }

        public Color RandColor => this.GetRandColor();

        public ObservableCollection<Account> Boarders
        {
            get { return boarders; }
            set { boarders = value; OnPropertyChanged(); }
        }

        private ObservableCollection<Account> boarders;

        private string Id;

        public MyBoardersViewModel()
        {
            this.ViewProfile = new Command(OnViewProfile);
            this.SelectedChange = new Command(SelectedItemChange);
            this.RemoveCommand = new Command(OnRemove);
        }

        private async void OnViewProfile(object obj)
        {
            Account account = (Account)obj;
        }
        private async void OnRemove(object obj)
        {
            Account account = (Account)obj;
        }

        private async void SelectedItemChange(object obj)
        {
            string searchText = obj as string;
            var sBoarders = await GetBoarders();
            if (searchText != string.Empty)
            {
                Boarders = new ObservableCollection<Account>(sBoarders.Where(w => w.FirstName.StartsWith(searchText)));
            }
            else
                Boarders = sBoarders;
        }

        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            if (query.Count > 0)
                this.Id = HttpUtility.UrlDecode(query["Id"]);
            this.SetDataBinding();
        }

        private async void SetDataBinding()
        {
            this.Boarders = await this.GetBoarders();
        }

        private async Task<ObservableCollection<Account>> GetBoarders()
        {
            ObservableCollection<Account> students = new ObservableCollection<Account>();
            var bookings = await App.Database.SearchBookingByStatusOwner(this.Id);
            if (bookings.Count > 0)
            {
                foreach (Booking booked in bookings)
                {
                    var student = await App.Database.GetAccountAsync(booked.StudentId);
                    students.Add(student);
                }
            }
            return students;
        }

        private Color GetRandColor()
        {
            System.Random rand = new System.Random();
            Color color = Color.FromRgb(rand.Next(256), rand.Next(256), rand.Next(256));
            return color;
        }
    }
}
