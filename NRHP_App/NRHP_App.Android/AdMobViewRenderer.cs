using Android.Content;
using Android.Gms.Ads;
using NRHP_App;
using NRHP_App.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(AdMobView), typeof(AdMobRenderer))]
namespace NRHP_App.Droid
{
    public class AdMobRenderer : ViewRenderer<AdMobView, AdView>
    {
        public AdMobRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<AdMobView> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                var adView = new AdView(Context)
                {
                    AdSize = AdSize.Banner,
                    AdUnitId = Element.AdUnitId
                };

                var requestbuilder = new AdRequest.Builder();

                adView.LoadAd(requestbuilder.Build());

                SetNativeControl(adView);
            }
        }
    }
}