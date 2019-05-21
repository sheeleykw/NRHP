using System.Collections.Generic;
using Xamarin.Forms;

namespace NRHP_App
{
    public partial class App : Application
    {
        public static DataPointDatabase database;
        public static string currentPinRefNum = null;
        public static MainPage mainPage = new MainPage();

        public App(string dbPath)
        {
            InitializeComponent();
            database = new DataPointDatabase(dbPath);
            MainPage = mainPage;
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
