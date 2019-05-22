using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using System.IO;

namespace NRHP_App.Droid
{
    [Activity(Label = "NRHP_App", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            string targetPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var path = Path.Combine(targetPath, "MapItems.db");
            if (!File.Exists(path))
            {
                using (Stream input = Assets.Open("MapItems.db"))
                {
                    using (var fs = new FileStream(path, FileMode.Create))
                    {
                        input.CopyTo(fs);
                    }
                }
            }

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            Plugin.CurrentActivity.CrossCurrentActivity.Current.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            global::Xamarin.FormsMaps.Init(this, savedInstanceState);
            LoadApplication(new App(path));
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}