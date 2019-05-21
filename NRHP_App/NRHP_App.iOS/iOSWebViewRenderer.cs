using System.IO;
using System.Net;
using Foundation;
using NRHP_App;
using NRHP_App.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer (typeof(PdfViewer), typeof(iOSWebViewRenderer))]
namespace NRHP_App.iOS
{
    class iOSWebViewRenderer : ViewRenderer<PdfViewer, UIWebView>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<PdfViewer> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
            }

            if (e.NewElement != null)
            {
                var PdfViewer = Element as PdfViewer;
                string fileName = Path.Combine(NSBundle.MainBundle.BundlePath, string.Format("Content/{0}", WebUtility.UrlEncode(PdfViewer.pdfUri)));
                Control.LoadRequest(new NSUrlRequest(new NSUrl(fileName, false)));
                Control.ScalesPageToFit = true;
            }
        }
    }
}