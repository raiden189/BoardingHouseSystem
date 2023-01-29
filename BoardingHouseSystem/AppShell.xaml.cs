using BoardingHouseSystem.Data;
using BoardingHouseSystem.ViewModels;
using BoardingHouseSystem.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace BoardingHouseSystem
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            //Routing.RegisterRoute(nameof(Database), typeof(Database));
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
