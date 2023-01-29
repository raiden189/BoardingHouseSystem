using BoardingHouseSystem.ViewModels;
using System.Collections.Generic;
using System.Net.Http;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BoardingHouseSystem.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OwnerPage : ContentPage
    {
        public OwnerPage()
        {
            InitializeComponent();
            this.BindingContext = new OwnerPageViewModel();
        }
    }
}