using BoardingHouseSystem.Data;
using BoardingHouseSystem.Models;
using BoardingHouseSystem.Services;
using BoardingHouseSystem.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using S = System.IO;

namespace BoardingHouseSystem
{
    public partial class App : Application
    {
        private static Database database;

        public static Database Database
        {
            get 
            {
                if (database == null)
                    database = new Database(S.Path.Combine(Environment.GetFolderPath(
                        Environment.SpecialFolder.LocalApplicationData), "login"));
                return database;
            }
        }

        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
