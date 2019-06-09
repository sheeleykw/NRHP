using System;
using System.Collections.Generic;
using System.IO;
using Xamarin.Forms;

namespace NRHP_App
{
    public partial class App : Application
    {
        public static MapPointDatabase mapDatabase;
        public static DataPointDatabase itemDatabase;
        public static string currentPinRefNum = null;
        public static bool updatedFavorites = false;
        public static MainPage mainPage = new MainPage();
        public static NavigationPage navPage = new NavigationPage(mainPage);

        public App(string mapDBPath, string itemDBPath)
        {
            InitializeComponent();

            mapDatabase = new MapPointDatabase(mapDBPath);
            itemDatabase = new DataPointDatabase(itemDBPath);

            //navPage.BarBackgroundColor = Color.FromHex("2699FB");
            MainPage = navPage;
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {

        }
    }
}
