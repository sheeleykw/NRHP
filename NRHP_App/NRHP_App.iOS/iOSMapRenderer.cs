using NRHP_App.iOS;
using NRHP_App;
using Xamarin.Forms;
using Xamarin.Forms.Maps.iOS;
using UIKit;
using Xamarin.Forms.Platform.iOS;
using MapKit;
using System;
using System.Threading.Tasks;
using CoreGraphics;

[assembly: ExportRenderer(typeof(NRHPMap), typeof(IOSMapRenderer))]
namespace NRHP_App.iOS
{
    public class IOSMapRenderer : MapRenderer
    {
        MKMapView nativeMap;
        UITapGestureRecognizer tapGesture;

        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                var formsMap = (NRHPMap)e.NewElement;
                nativeMap = Control as MKMapView;

                nativeMap.DidSelectAnnotationView += SelectPoint;
                nativeMap.DidDeselectAnnotationView += DeselectPoint;
                App.mainPage.SearchCompleted += LoadAnnotationElement;
                tapGesture = new UITapGestureRecognizer(new Action(LoadDetailPage));
            }
        }

        private async void LoadAnnotationElement(object sender, MapPoint e)
        {
            var markerFound = false;

            if (e != null)
            {
                while (!markerFound)
                {
                    foreach (IMKAnnotation annotation in nativeMap.Annotations)
                    {
                        if (annotation.GetTitle().Equals(e.Name))
                        {
                            nativeMap.SelectAnnotation(annotation, true);
                            markerFound = true;

                            App.currentPinRefNum = (await App.mapDatabase.GetRefNumAsync(annotation.GetTitle(), annotation.Coordinate.Latitude, annotation.Coordinate.Longitude)).RefNum;
                            App.mainPage.SwitchDetailPageButton();
                        }
                    }
                    await Task.Delay(150);
                }
            }
        }

        //Called when the annotation view of the selected point is tapped.
        private void LoadDetailPage()
        {
            App.mainPage.OpenDetailPage();
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

                e.View.AddGestureRecognizer(tapGesture);
            }
        }

        private void DeselectPoint(object sender, MKAnnotationViewEventArgs e)
        {
            e.View.RemoveGestureRecognizer(tapGesture);
            App.currentPinRefNum = null;
            App.mainPage.SwitchDetailPageButton();
        }

        protected override MKAnnotationView GetViewForAnnotation(MKMapView mapView, IMKAnnotation annotation)
        {
            if (annotation is MKPointAnnotation)
            {
                MKAnnotationView annotationView = new MKAnnotationView(annotation, annotation.ToString());

                annotationView.Image = UIImage.FromBundle("s");
                annotationView.CalloutOffset = new CGPoint(0, 0);
                annotationView.CanShowCallout = true;

                return annotationView;
            }
            else
            {
                return base.GetViewForAnnotation(mapView, annotation);
            }
        }
    }
}