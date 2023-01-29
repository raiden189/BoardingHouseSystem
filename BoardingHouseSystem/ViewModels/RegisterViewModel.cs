using BoardingHouseSystem.Models;
using BoardingHouseSystem.Views;
using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace BoardingHouseSystem.ViewModels
{
    public class RegisterViewModel : INotifyPropertyChanged
    {
        public Command ContinueCommand { get; set; }

        public Command CancelCommand { get; set; }

        public Command TakePicCommand { get; set; }

        public Command SelectPicCommand { get; set; }

        public ImageSource PhotoSource 
        {
            get 
            {
                return imgSource;   
            }
            set 
            {
                imgSource = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private ImageSource imgSource;

        private string imagePath;

        public RegisterViewModel()
        {
            ContinueCommand = new Command(OnContinueClicked);
            CancelCommand = new Command(OnCancelClicked);
            TakePicCommand = new Command(OnTakePicture);
            SelectPicCommand= new Command(OnSelectPicture);
            imgSource = ImageSource.FromResource("BoardingHouseSystem.Resources.Images.nophoto.png");
        }

        private async void OnSelectPicture(object obj)
        {
            try
            {
                await CrossMedia.Current.Initialize();
                if (!CrossMedia.Current.IsPickPhotoSupported)
                    await Shell.Current.DisplayAlert("Error", "Image not supported.", "OK");

                var mediaOption = new PickMediaOptions()
                {
                    PhotoSize = PhotoSize.Medium
                };
                MediaFile mediaFile = await CrossMedia.Current.PickPhotoAsync(mediaOption);
                MemoryStream memoryStream = new MemoryStream();
                if (mediaFile == null) return;
                imagePath = mediaFile.Path;
                PhotoSource = ImageSource.FromStream(() => mediaFile.GetStream());
            }
            catch (Exception)
            {
                await Shell.Current.DisplayAlert("Error", "Image not supported.", "OK");
            }
        }

        private async void OnTakePicture(object obj)
        {
            try
            {
                await CrossMedia.Current.Initialize();
                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                    await Shell.Current.DisplayAlert("Error", "No Camera available.", "OK");

                MediaFile mediaFile = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                {
                    Directory = "Images",
                    Name = "test.jpg",
                    PhotoSize = PhotoSize.Full
                });

                MemoryStream memoryStream = new MemoryStream();
                if (mediaFile == null) return;
                PhotoSource = ImageSource.FromStream(() =>
                {
                    imagePath = mediaFile.Path;
                    return mediaFile.GetStream();
                });
            }
            catch (Exception)
            {
                await Shell.Current.DisplayAlert("Error", "No Camera available.", "OK");
            }
        }

        private async void OnContinueClicked(object obj)
        {
            Account account = obj as Account;
            account.Photo = this.imagePath;
            await Shell.Current.GoToAsync($"//{nameof(RegisterUserPage)}?account=" +
                $"{JsonConvert.SerializeObject(account)}");
        }

        private async void OnCancelClicked(object obj)
        {
            Application.Current.MainPage = new AppShell();
        }
    }
}
