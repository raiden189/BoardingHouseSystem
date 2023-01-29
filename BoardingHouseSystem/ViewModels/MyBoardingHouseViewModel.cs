using BoardingHouseSystem.Data;
using BoardingHouseSystem.Models;
using BoardingHouseSystem.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
//using static System.Net.Mime.MediaTypeNames;

namespace BoardingHouseSystem.ViewModels
{
    public class MyBoardingHouseViewModel : INotifyPropertyChanged, IQueryAttributable
    {
        public Command FilePickerCommand { get; }

        public Command CancelCommand { get; }

        public Command SubmitCommand { get; }

        public Command DeleteImageCommand { get; }

        public Command LoadMapCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
        public ObservableCollection<ImagePath> Images
        {
            get { return images; }
            set { images = value; OnPropertyChanged(); }
        }

        private ObservableCollection<ImagePath> images;

        private string Id;

        public MyBoardingHouseViewModel()
        {
            FilePickerCommand = new Command(PickAndShowMultiple);
            DeleteImageCommand = new Command(OnDeleteImage);
            LoadMapCommand = new Command(LoadMap);
            SubmitCommand = new Command(OnSubmit);
            CancelCommand= new Command(OnCancel);
        }

        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            if (query.Count > 0)
                this.Id = HttpUtility.UrlDecode(query["Id"]);
        }

        public async void OnDeleteImage(object obj)
        {
            ImagePath imgPath = obj as ImagePath;
            this.Images.Remove(imgPath);
        }

        private async void OnCancel(object obj)
        {
            Application.Current.MainPage = new AppShell();
        }

        public async void OnSubmit(object obj)
        {
            Boarding boarding = (Boarding)obj;
            boarding.id = this.Id;
            List<ImagePath> newImages = new List<ImagePath>();
            Xamarin.Forms.Maps.Position position = await MapAPI.GetPossiblePositionByAddress(boarding.Address);
            if (position != null)
            {
                boarding.DistanceRange = (int)await MapAPI.GetDistanceByPosition(position);
                foreach (ImagePath item in Images)
                {
                    string dlLink = await FireBaseApi.UploadFile(FileHandler.GenerateNewFileName(item.Path), item.Path);
                    newImages.Add(new ImagePath { ImageId = this.Id, Path = dlLink });
                    FileHandler.DisposeFile(item.Path);
                }
                int savedData = await App.Database.SaveBoardingAsync(boarding, newImages);
                if (savedData > 0)
                    await Shell.Current.DisplayAlert("Success", "Boarding House successfully saved.", "OK");
                else
                    await Shell.Current.DisplayAlert("Error", "Error on saving boarding house.", "OK");

                await Shell.Current.GoToAsync($"//{nameof(StudentProfilePage)}?Id={this.Id}");
            }
            else
                await Shell.Current.DisplayAlert("Error", "Cannot find specified address.", "OK");
        }

        public async void LoadMap(object obj)
        {
            if (Device.RuntimePlatform.Equals(Device.iOS))
                await Launcher.OpenAsync("https://www.google.com/maps/dir/Jose+Rizal+Memorial+State+University+-+Main+Campus,+MC4F%2B936,+Gov.+Sta.+Cruz,+Guading+Adasa+St,+Dapitan+City,+Zamboanga+del+Norte/Don+Hospicio+Ochhtorena+St,+Dapitan+City,+Zamboanga+del+Norte/@8.6571671,123.4202828,17z/data=!3m1!4b1!4m13!4m12!1m5!1m1!1s0x3254bf4454a8bf07:0xfc69ea2ff39b5bfd!2m2!1d123.422625!2d8.6559176!1m5!1m1!1s0x3254bf4f3badc66f:0x4c283a3a8b8b2c78!2m2!1d123.4224366!2d8.6583828");
            else if (Device.RuntimePlatform.Equals(Device.Android))
                await Launcher.OpenAsync("https://www.google.com/maps/dir/Jose+Rizal+Memorial+State+University+-+Main+Campus,+MC4F%2B936,+Gov.+Sta.+Cruz,+Guading+Adasa+St,+Dapitan+City,+Zamboanga+del+Norte/Don+Hospicio+Ochhtorena+St,+Dapitan+City,+Zamboanga+del+Norte/@8.6571671,123.4202828,17z/data=!3m1!4b1!4m13!4m12!1m5!1m1!1s0x3254bf4454a8bf07:0xfc69ea2ff39b5bfd!2m2!1d123.422625!2d8.6559176!1m5!1m1!1s0x3254bf4f3badc66f:0x4c283a3a8b8b2c78!2m2!1d123.4224366!2d8.6583828");
        }

        public async void PickAndShowMultiple()
        {
            try
            {
                ObservableCollection<ImagePath> fileImages = new ObservableCollection<ImagePath>();
                string filePath = string.Empty;
                var res = await FilePicker.PickMultipleAsync();
                if (res != null)
                {
                    foreach (var result in res)
                    {
                        if (result.FileName.EndsWith("jpeg", StringComparison.OrdinalIgnoreCase) ||
                            result.FileName.EndsWith("jpg", StringComparison.OrdinalIgnoreCase) ||
                            result.FileName.EndsWith("png", StringComparison.OrdinalIgnoreCase))
                            filePath = result.FullPath;

                        fileImages.Add(new ImagePath { Path = filePath });
                    }
                }
                Images = fileImages;
            }
            catch (Exception)
            {
                Images = null;
            }
        }
    }
}
