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

        public static List<Pin> currentPins = new List<Pin>();
        public static List<ObjectBind> stateList = new List<ObjectBind>();
        public static List<ObjectBind> filterList = new List<ObjectBind>();

        public static MainPage mainPage = new MainPage();
        public static NavigationPage navPage = new NavigationPage(mainPage);

        public App(string mapDBPath, string itemDBPath, string cityDBPath)
        {
            InitializeComponent();

            //User location checking.
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            if (userPosition == null)
            {
                userPosition = new Location(0.000000, 0.000000);
            }
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //Database connection and variable assignment.
            mapDatabase = new MapPointDatabase(mapDBPath);
            itemDatabase = new DataPointDatabase(itemDBPath);
            cityDatabase = new CityPointDatabase(cityDBPath);

            //MainPage and NavPage settings.
            navPage.BarBackgroundColor = Color.White;
            MainPage = new MasterPage(navPage);

            //Bindings for filter properties held within the filterList variable. ORDER OF ADDING MUST MATCH FOR THE CALLING OF THE BINDINGS.
            filterList.Add(new ObjectBind("Building", true));
            filterList.Add(new ObjectBind("Structure", true));
            filterList.Add(new ObjectBind("Site", true));
            filterList.Add(new ObjectBind("Object", true));


            //Bindings for state permissions held within the stateList variable.
            stateList.Add(new ObjectBind("Alaska", false));
            stateList.Add(new ObjectBind("American Samoa", false));
            stateList.Add(new ObjectBind("Arizona", false));
            stateList.Add(new ObjectBind("Arkansas", false));
            stateList.Add(new ObjectBind("California", false));
            stateList.Add(new ObjectBind("Colorado", false));
            stateList.Add(new ObjectBind("Connecticut", false));
            stateList.Add(new ObjectBind("Delaware", false));
            stateList.Add(new ObjectBind("District Of Columbia", false));
            stateList.Add(new ObjectBind("Fed. States", false));
            stateList.Add(new ObjectBind("Florida", false));
            stateList.Add(new ObjectBind("Georgia", false));
            stateList.Add(new ObjectBind("Guam", false));
            stateList.Add(new ObjectBind("Hawaii", false));
            stateList.Add(new ObjectBind("Idaho", false));
            stateList.Add(new ObjectBind("Illinois", false));
            stateList.Add(new ObjectBind("Indiana", false));
            stateList.Add(new ObjectBind("Iowa", false));
            stateList.Add(new ObjectBind("Kansas", false));
            stateList.Add(new ObjectBind("Kentucky", false));
            stateList.Add(new ObjectBind("Louisiana", false));
            stateList.Add(new ObjectBind("Maine", false));
            stateList.Add(new ObjectBind("Marshall Islands", false));
            stateList.Add(new ObjectBind("Maryland", false));
            stateList.Add(new ObjectBind("Massachusetts", false));
            stateList.Add(new ObjectBind("Michigan", false));
            stateList.Add(new ObjectBind("Minnesota", false));
            stateList.Add(new ObjectBind("Mississippi", false));
            stateList.Add(new ObjectBind("Missouri", false));
            stateList.Add(new ObjectBind("Montana", false));
            stateList.Add(new ObjectBind("Morocco", false));
            stateList.Add(new ObjectBind("N. Mariana Islands", false));
            stateList.Add(new ObjectBind("Nebraska", false));
            stateList.Add(new ObjectBind("Nevada", false));
            stateList.Add(new ObjectBind("New Hampshire", false));
            stateList.Add(new ObjectBind("New Jersey", false));
            stateList.Add(new ObjectBind("New Mexico", false));
            stateList.Add(new ObjectBind("New York", false));
            stateList.Add(new ObjectBind("North Carolina", false));
            stateList.Add(new ObjectBind("North Dakota", false));
            stateList.Add(new ObjectBind("Not Listed", false));
            stateList.Add(new ObjectBind("Ohio", false));
            stateList.Add(new ObjectBind("Oklahoma", false));
            stateList.Add(new ObjectBind("Oregon", false));
            stateList.Add(new ObjectBind("Palau", false));
            stateList.Add(new ObjectBind("Pennsylvania", false));
            stateList.Add(new ObjectBind("Puerto Rico", false));
            stateList.Add(new ObjectBind("Rhode Island", false));
            stateList.Add(new ObjectBind("South Carolina", false));
            stateList.Add(new ObjectBind("South Dakota", false));
            stateList.Add(new ObjectBind("Tennessee", false));
            stateList.Add(new ObjectBind("Texas", false));
            stateList.Add(new ObjectBind("Utah", false));
            stateList.Add(new ObjectBind("Vermont", false));
            stateList.Add(new ObjectBind("Virgin Islands", false));
            stateList.Add(new ObjectBind("Virginia", false));
            stateList.Add(new ObjectBind("Washington", false));
            stateList.Add(new ObjectBind("West Virginia", false));
            stateList.Add(new ObjectBind("Wisconsin", false));
            stateList.Add(new ObjectBind("Wyoming", false));
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
