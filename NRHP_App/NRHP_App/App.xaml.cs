using System;
using Xamarin.Forms;
using System.IO;
using System.Reflection;
using NRHP_App.Data;
using NRHP_App.Models;

namespace NRHP_App
{
    public partial class App : Application
    {
        //static DataPointDatabase database;

        //public static DataPointDatabase Database
        //{
        //    get
        //    {
        //        if (database == null)
        //        {
        //            database = new DataPointDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "DataPoints.db3"));

        //            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(App)).Assembly;
        //            Stream stream = assembly.GetManifestResourceStream("NRHP_App.AllElements.txt");
        //            using (var reader = new StreamReader(stream))
        //            {
        //                string lineData;
        //                while ((lineData = reader.ReadLine()) != null)
        //                {
        //                    var separatedData = lineData.Split('\t');
        //                    if (!(separatedData[2] == "" || separatedData[3] == ""))
        //                    {
        //                        var dataPoint = new DataPoint
        //                        {
        //                            RefNum = separatedData[0],
        //                            Name = separatedData[1],
        //                            Address = "NULL",
        //                            Latitude = Convert.ToDouble(separatedData[2]),
        //                            Longitude = Convert.ToDouble(separatedData[3]),
        //                            Category = separatedData[4]
        //                        };
        //                        database.SavePointAsync(dataPoint);
        //                    }
        //                }
        //            }
        //        }
        //        return database;
        //    }
        //}

        public App()
        {
            InitializeComponent();
            MainPage = new MainPage();
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
            // Handle when your app resumes
        }
    }
}
