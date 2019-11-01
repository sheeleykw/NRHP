using System;
using Foundation;
using UIKit;
using System.IO;

namespace NRHP_App.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            var mapPath = NSBundle.MainBundle.PathForResource("MapItems", "db");
            var itemPath = NSBundle.MainBundle.PathForResource("ItemDetails", "db");
            var cityPath = NSBundle.MainBundle.PathForResource("CityLocations", "db");

            Google.MobileAds.MobileAds.Configure("ca-app-pub-3281339494640251~8481947871");
            global::Xamarin.Forms.Forms.Init();
            global::Xamarin.FormsMaps.Init();
            LoadApplication(new App(mapPath, itemPath, cityPath));

            return base.FinishedLaunching(app, options);
        }
    }
}
