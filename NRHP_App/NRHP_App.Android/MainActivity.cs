using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using System.IO;
using Plugin.Permissions;

namespace NRHP_App.Droid
{
    [Activity(Label = "Sytes", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            string targetPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var mapPath = Path.Combine(targetPath, "MapItems.db");
            if (!File.Exists(mapPath))
            {
                using (Stream input = Assets.Open("MapItems.db"))
                {
                    using (var fs = new FileStream(mapPath, FileMode.Create))
                    {
                        input.CopyTo(fs);
                    }
                }
            }

            var itemPath = Path.Combine(targetPath, "ItemDetails.db");
            if (!File.Exists(itemPath))
            {
                using (Stream input = Assets.Open("ItemDetails.db"))
                {
                    using (var fs = new FileStream(itemPath, FileMode.Create))
                    {
                        input.CopyTo(fs);
                    }
                }
            }

            var cityPath = Path.Combine(targetPath, "CityLocations.db");
            if (!File.Exists(cityPath))
            {
                using (Stream input = Assets.Open("CityLocations.db"))
                {
                    using (var fs = new FileStream(cityPath, FileMode.Create))
                    {
                        input.CopyTo(fs);
                    }
                }
            }

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            Plugin.CurrentActivity.CrossCurrentActivity.Current.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            global::Xamarin.FormsMaps.Init(this, savedInstanceState);
            LoadApplication(new App(mapPath, itemPath, cityPath));
        }
        //public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        //{
        //    Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

        //    base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        //}

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}