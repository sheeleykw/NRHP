﻿using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Essentials;

namespace NRHP_App
{
    public partial class App : Application
    {
        public static MapPointDatabase mapDatabase;
        public static DataPointDatabase itemDatabase;
        public static CityPointDatabase cityDatabase;
        public static Location userPosition = Geolocation.GetLastKnownLocationAsync().Result;
        public static string currentPinRefNum = null;
        public static bool updatedFavorites = false;
        public static string currentMapSearchTerm = "";

        public static List<Pin> currentPins = new List<Pin>();

        public static MainPage mainPage = new MainPage();
        public static NavigationPage navPage = new NavigationPage(mainPage);

        public App(string mapDBPath, string itemDBPath, string cityDBPath)
        {
            InitializeComponent();

            mapDatabase = new MapPointDatabase(mapDBPath);
            itemDatabase = new DataPointDatabase(itemDBPath);
            cityDatabase = new CityPointDatabase(cityDBPath);

            navPage.BarBackgroundColor = Color.White;
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
