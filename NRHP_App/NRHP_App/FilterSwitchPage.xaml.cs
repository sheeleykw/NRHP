using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NRHP_App
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FilterSwitchPage : ContentPage
    {
        Switch buildingSwitch = new Switch { IsToggled = true };
        Switch structureSwitch = new Switch { IsToggled = true };
        Switch siteSwitch = new Switch { IsToggled = true };
        Switch objectSwitch = new Switch { IsToggled = true };
        StackLayout mainLayout = new StackLayout();

        public FilterSwitchPage()
        {
            InitializeComponent();

            buildingSwitch.Toggled += (sender, e) =>
            {
                App.filterList[0].objectState = e.Value;
                App.mainPage.FilterChange();
            };
            structureSwitch.Toggled += (sender, e) =>
            {
                App.filterList[1].objectState = e.Value;
                App.mainPage.FilterChange();
            };
            siteSwitch.Toggled += (sender, e) =>
            {
                App.filterList[2].objectState = e.Value;
                App.mainPage.FilterChange();
            };
            objectSwitch.Toggled += (sender, e) =>
            {
                App.filterList[3].objectState = e.Value;
                App.mainPage.FilterChange();
            };

            mainLayout.Children.Add(buildingSwitch);
            mainLayout.Children.Add(structureSwitch);
            mainLayout.Children.Add(siteSwitch);
            mainLayout.Children.Add(objectSwitch);

            Content = mainLayout;
        }
    }
}