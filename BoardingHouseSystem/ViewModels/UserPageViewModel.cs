using BoardingHouseSystem.Models;
using BoardingHouseSystem.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Xamarin.Forms;

namespace BoardingHouseSystem.ViewModels
{
    public class UserPageViewModel : INotifyPropertyChanged, IQueryAttributable
    {
        public string Id { get; set; }
        public Command ViewBoarding { get; }

        public Command LoadMap { get; }

        public Command ViewProfile { get; }

        public Command SelectedChange { get; }

        public Command SearchCommand { get; }

        public Command RandColor { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ObservableCollection<BoardingHouse> BoardingHouses
        {
            get { return boardingHouses; }
            set { boardingHouses = value; OnPropertyChanged(); }
        }

        public ObservableCollection<BoardingHouse> boardingHouses;

        public UserPageViewModel() 
        {
            this.boardingHouses = GetBoardingHouses();
            ViewBoarding = new Command(OnViewBoardingClick);
            LoadMap = new Command(OnLoadMapClick);
            SelectedChange = new Command(SelectedItemChange);
            ViewProfile = new Command(OnViewProfile);
            SearchCommand = new Command(OnSearchCommand);
            LoadPage();
        }

        public async void LoadPage()
        {
            //Account account = await App.Database.GetAccountAsync(this.Id);
            //if (!account.isStudent)
                //await Shell.Current.GoToAsync($"//{nameof(OwnerPage)}?Id={Id}");
        }

        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            if (query.Count > 0)
                this.Id = HttpUtility.UrlDecode(query["Id"]);
        }

        private async void OnSearchCommand(object obj)
        {
            ObservableCollection<BoardingHouse> localBoarding = new ObservableCollection<BoardingHouse>();
            Search searchValues = (Search)obj;
            switch (searchValues.SelectedPriceType)
            {
                case "Lowest":
                    if (searchValues.SelectedDistanceType != -1)
                        BoardingHouses = new ObservableCollection<BoardingHouse>(GetBoardingHouses()
                            .Where(w => w.DistanceRange == searchValues.SelectedDistanceType)
                            .OrderBy(o => o.MonthlyPayment));
                    else
                        BoardingHouses = new ObservableCollection<BoardingHouse>(GetBoardingHouses()
                            .OrderBy(o => o.MonthlyPayment));
                    break;
                case "Highest":
                    if (searchValues.SelectedDistanceType != -1)
                        BoardingHouses = new ObservableCollection<BoardingHouse>(GetBoardingHouses()
                            .Where(w => w.DistanceRange == searchValues.SelectedDistanceType)
                            .OrderByDescending(o => o.MonthlyPayment));
                    else
                        BoardingHouses = new ObservableCollection<BoardingHouse>(GetBoardingHouses()
                            .OrderByDescending(o => o.MonthlyPayment));
                    break;
                default:
                    if (searchValues.SelectedDistanceType != -1)
                        BoardingHouses = new ObservableCollection<BoardingHouse>(GetBoardingHouses()
                            .Where(w => w.DistanceRange == searchValues.SelectedDistanceType));
                    else
                        BoardingHouses = new ObservableCollection<BoardingHouse>(GetBoardingHouses());
                    break;
            }
        }

        private async void OnViewBoardingClick(object obj)
        {
            BoardingHouse boarding = (BoardingHouse)obj;
            await Shell.Current.GoToAsync($"//{nameof(StudentBoardingView)}?sId={this.Id}&bId={boarding.Id}");
        }

        private async void OnLoadMapClick(object obj)
        {

        }

        private async void OnViewProfile(object obj)
        {
            await Shell.Current.GoToAsync($"//{nameof(StudentProfilePage)}?Id={Id}");
        }

        private async void SelectedItemChange(object obj)
        {
            string searchText = obj as string;
            if (searchText != string.Empty)
                BoardingHouses = new ObservableCollection<BoardingHouse>(GetBoardingHouses().Where(w => w.Name.StartsWith(searchText)));
            else
                BoardingHouses = GetBoardingHouses();
        }

        private ObservableCollection<BoardingHouse> GetBoardingHouses()
        {
            Dictionary<Boarding, List<ImagePath>> keyValuePairs = App.Database.GetBoardingHouseWithImagesAsync();
            ObservableCollection<BoardingHouse> boardingHouses = new ObservableCollection<BoardingHouse>();
            foreach (var item in keyValuePairs)
            {
                boardingHouses.Add(new BoardingHouse
                {
                    Id = item.Key.id,
                    Name = item.Key.Name,
                    Address = item.Key.Address,
                    ContactNumber = item.Key.ContactNumber,
                    ContactPerson = item.Key.ContactPerson,
                    MonthlyPayment = item.Key.MonthlyPayment,
                    DistanceRange = item.Key.DistanceRange,
                    ColorDesign = GetRandColor(),
                    ThumbNail = item.Value.FirstOrDefault().Path
                });
            }
            return boardingHouses;
        }

        private Color GetRandColor()
        {
            Random rand = new Random();
            Color color = Color.FromRgb(rand.Next(256), rand.Next(256), rand.Next(256));
            return color;
        }
    }
}
