using NRHP_App.iOS;
using NRHP_App;
using Xamarin.Forms;
using Xamarin.Forms.Maps.iOS;
using UIKit;
using Xamarin.Forms.Platform.iOS;
using MapKit;
using System;

[assembly: ExportRenderer(typeof(NRHPMap), typeof(IOSMapRenderer))]
namespace NRHP_App.iOS
{
    public class IOSMapRenderer : MapRenderer
    {
        MKMapView nativeMap;

        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                nativeMap = Control as MKMapView;
                if (nativeMap != null)
                {
                    //nativeMap.RemoveAnnotations(nativeMap.Annotations);
                    //nativeMap.GetViewForAnnotation = null;
                    //nativeMap.CalloutAccessoryControlTapped -= OnCalloutAccessoryControlTapped;
                    //nativeMap.DidSelectAnnotationView -= OnDidSelectAnnotationView;
                    //nativeMap.DidDeselectAnnotationView -= OnDidDeselectAnnotationView;
                }
            }

            if (e.NewElement != null)
            {
                var formsMap = (NRHPMap)e.NewElement;
                nativeMap = Control as MKMapView;

                nativeMap.DidSelectAnnotationView += SelectPoint;
                nativeMap.DidDeselectAnnotationView += DeselectPoint;

                //nativeMap.SelectedAnnotation;
                //nativeMap.GetViewForAnnotation = GetViewForAnnotation;
                nativeMap.CalloutAccessoryControlTapped += PinTapped;
            }
        }

        private async void SelectPoint(object sender, MKAnnotationViewEventArgs e)
        {
            var annotation = e.View.Annotation;
            if (annotation is MKPointAnnotation)
            {
                var moveCamera = new MKMapCamera
                {
                    CenterCoordinate = annotation.Coordinate,
                    Altitude = nativeMap.Camera.Altitude,
                    Heading = nativeMap.Camera.Heading
                };
                nativeMap.SetCamera(moveCamera, true);
                App.currentPinRefNum = (await App.mapDatabase.GetRefNumAsync(annotation.GetTitle(), annotation.Coordinate.Latitude, annotation.Coordinate.Longitude)).RefNum;
                App.mainPage.SwitchDetailPageButton();
            }
        }

        private void DeselectPoint(object sender, MKAnnotationViewEventArgs e)
        {
            App.currentPinRefNum = null;
            App.mainPage.SwitchDetailPageButton();
        }

        private void PinTapped(object sender, EventArgs e)
        {
            Console.WriteLine("Hello");
        }
    }
}