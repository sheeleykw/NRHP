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
        Switch buildingSwitch = new Switch { IsToggled = true, Scale = 1.7, OnColor = Color.FromHex("5f90fe") };
        Switch structureSwitch = new Switch { IsToggled = true, Scale = 1.7, OnColor = Color.FromHex("5f90fe") };
        Switch siteSwitch = new Switch { IsToggled = true, Scale = 1.7, OnColor = Color.FromHex("5f90fe") };
        Switch objectSwitch = new Switch { IsToggled = true, Scale = 1.7, OnColor = Color.FromHex("5f90fe") };
        Label buildingLabel = new Label { Text = "Buildings: ", FontSize = 20 };
        Label structureLabel = new Label { Text = "Structures: ", FontSize = 20 };
        Label siteLabel = new Label { Text = "Sites: ", FontSize = 20 };
        Label objectLabel = new Label { Text = "Objects: ", FontSize = 20 };

        Constraint switchXConstraint = Constraint.RelativeToParent((Parent) =>
        {
            return (0.42 * Parent.Width);
        });
        Constraint labelXConstraint = Constraint.RelativeToParent((Parent) =>
        {
            return (0.06 * Parent.Width);
        });

        RelativeLayout mainLayout = new RelativeLayout();

        public FilterSwitchPage()
        {
            InitializeComponent();
            //MinimumWidthRequest = 100;
            //WidthRequest = 150;

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

            mainLayout.Children.Add(buildingSwitch, switchXConstraint,
            Constraint.RelativeToParent((Parent) =>
            {
                return Parent.Height * 0.03;
            }));
            mainLayout.Children.Add(buildingLabel, labelXConstraint, Constraint.RelativeToView(buildingSwitch, (Parent, view) =>
            {
                return view.Y - 2;
            }));

            mainLayout.Children.Add(structureSwitch, switchXConstraint,
            Constraint.RelativeToView(buildingSwitch, (Parent, view) =>
            {
                return view.Y + view.Height + 20;
            }));
            mainLayout.Children.Add(structureLabel, labelXConstraint, Constraint.RelativeToView(structureSwitch, (Parent, view) =>
            {
                return view.Y - 2;
            }));

            mainLayout.Children.Add(siteSwitch, switchXConstraint,
            Constraint.RelativeToView(structureSwitch, (Parent, view) =>
            {
                return view.Y + view.Height + 20;
            }));
            mainLayout.Children.Add(siteLabel, labelXConstraint, Constraint.RelativeToView(siteSwitch, (Parent, view) =>
            {
                return view.Y - 2;
            }));

            mainLayout.Children.Add(objectSwitch, switchXConstraint,
            Constraint.RelativeToView(siteSwitch, (Parent, view) =>
            {
                return view.Y + view.Height + 20;
            }));
            mainLayout.Children.Add(objectLabel, labelXConstraint, Constraint.RelativeToView(objectSwitch, (Parent, view) =>
            {
                return view.Y - 2;
            }));

            Content = mainLayout;
        }
    }
}