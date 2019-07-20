using System.Collections.Generic;
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
        public static string currentPinRefNum;
        public static bool updatedFavorites;
        public static string currentMapSearchTerm = "";

        public static List<Pin> currentPins = new List<Pin>();
        public static List<StateBind> stateList = new List<StateBind>();

        public static MainPage mainPage = new MainPage();
        public static NavigationPage navPage = new NavigationPage(mainPage);

        public App(string mapDBPath, string itemDBPath, string cityDBPath)
        {
            InitializeComponent();

            if (userPosition == null)
            {
                userPosition = new Location(0.000000, 0.000000);
            }
            //try
            //{
            //    App.userPosition = Geolocation.GetLocationAsync(new GeolocationRequest(GeolocationAccuracy.Lowest)).Result;
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e.Message);
            //}

            mapDatabase = new MapPointDatabase(mapDBPath);
            itemDatabase = new DataPointDatabase(itemDBPath);
            cityDatabase = new CityPointDatabase(cityDBPath);

            navPage.BarBackgroundColor = Color.White;
            MainPage = new MasterPage(navPage);

            stateList.Add(new StateBind("Alaska", false));
            stateList.Add(new StateBind("American Samoa", false));
            stateList.Add(new StateBind("Arizona", false));
            stateList.Add(new StateBind("Arkansas", false));
            stateList.Add(new StateBind("California", false));
            stateList.Add(new StateBind("Colorado", false));
            stateList.Add(new StateBind("Connecticut", false));
            stateList.Add(new StateBind("Delaware", false));
            stateList.Add(new StateBind("District Of Columbia", false));
            stateList.Add(new StateBind("Fed. States", false));
            stateList.Add(new StateBind("Florida", false));
            stateList.Add(new StateBind("Georgia", false));
            stateList.Add(new StateBind("Guam", false));
            stateList.Add(new StateBind("Hawaii", false));
            stateList.Add(new StateBind("Idaho", false));
            stateList.Add(new StateBind("Illinois", false));
            stateList.Add(new StateBind("Indiana", false));
            stateList.Add(new StateBind("Iowa", false));
            stateList.Add(new StateBind("Kansas", false));
            stateList.Add(new StateBind("Kentucky", false));
            stateList.Add(new StateBind("Louisiana", false));
            stateList.Add(new StateBind("Maine", false));
            stateList.Add(new StateBind("Marshall Islands", false));
            stateList.Add(new StateBind("Maryland", false));
            stateList.Add(new StateBind("Massachusetts", false));
            stateList.Add(new StateBind("Michigan", false));
            stateList.Add(new StateBind("Minnesota", false));
            stateList.Add(new StateBind("Mississippi", false));
            stateList.Add(new StateBind("Missouri", false));
            stateList.Add(new StateBind("Montana", false));
            stateList.Add(new StateBind("Morocco", false));
            stateList.Add(new StateBind("N. Mariana Islands", false));
            stateList.Add(new StateBind("Nebraska", false));
            stateList.Add(new StateBind("Nevada", false));
            stateList.Add(new StateBind("New Hampshire", false));
            stateList.Add(new StateBind("New Jersey", false));
            stateList.Add(new StateBind("New Mexico", false));
            stateList.Add(new StateBind("New York", false));
            stateList.Add(new StateBind("North Carolina", false));
            stateList.Add(new StateBind("North Dakota", false));
            stateList.Add(new StateBind("Not Listed", false));
            stateList.Add(new StateBind("Ohio", false));
            stateList.Add(new StateBind("Oklahoma", false));
            stateList.Add(new StateBind("Oregon", false));
            stateList.Add(new StateBind("Palau", false));
            stateList.Add(new StateBind("Pennsylvania", false));
            stateList.Add(new StateBind("Puerto Rico", false));
            stateList.Add(new StateBind("Rhode Island", false));
            stateList.Add(new StateBind("South Carolina", false));
            stateList.Add(new StateBind("South Dakota", false));
            stateList.Add(new StateBind("Tennessee", false));
            stateList.Add(new StateBind("Texas", false));
            stateList.Add(new StateBind("Utah", false));
            stateList.Add(new StateBind("Vermont", false));
            stateList.Add(new StateBind("Virgin Islands", false));
            stateList.Add(new StateBind("Virginia", false));
            stateList.Add(new StateBind("Washington", false));
            stateList.Add(new StateBind("West Virginia", false));
            stateList.Add(new StateBind("Wisconsin", false));
            stateList.Add(new StateBind("Wyoming", false));
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
