using Android.Content;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using NRHP_App;
using NRHP_App.Droid;
using System.Net;

[assembly: ExportRenderer (typeof(PdfViewer), typeof(AndroidWebViewRenderer))]
namespace NRHP_App.Droid
{
    public class AndroidWebViewRenderer : WebViewRenderer
    {
        public AndroidWebViewRenderer(Context context) : base(context)
        {
        }
        protected override void OnElementChanged(ElementChangedEventArgs<WebView> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                var pdfViewer = Element as PdfViewer;
                Control.Settings.AllowUniversalAccessFromFileURLs = true;
                Control.LoadUrl(string.Format("file:///android_asset/pdfjs/web/viewer.html?file={0}", string.Format("file:///android_asset/Content/{0}", WebUtility.UrlEncode(pdfViewer.pdfUri))));
            }
        }
    }
}