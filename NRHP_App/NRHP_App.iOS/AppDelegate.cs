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

            //var targetPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            //var mapPath = Path.Combine(targetPath, "..", "Library");
            //if (!File.Exists(mapPath))
            //{
            //    var bundlePath = NSBundle.MainBundle.PathForResource("MapItems", "db");
            //    Console.WriteLine(bundlePath);
            //    Console.WriteLine(mapPath);
            //    File.Copy(bundlePath, mapPath, true);
            //}

            var itemPath = NSBundle.MainBundle.PathForResource("ItemDetails", "db");

            //var itemPath = Path.Combine(targetPath, "..", "Library");
            //if (!File.Exists(itemPath))
            //{
            //    var bundlePath = NSBundle.MainBundle.PathForResource("ItemDetails", "db");
            //    File.Copy(bundlePath, itemPath, true);
            //}

            global::Xamarin.Forms.Forms.Init();
            global::Xamarin.FormsMaps.Init();
            LoadApplication(new App(mapPath, itemPath));

            return base.FinishedLaunching(app, options);
        }
    }
}
