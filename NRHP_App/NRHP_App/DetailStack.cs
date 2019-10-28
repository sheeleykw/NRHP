using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace NRHP_App
{
    public class DetailStack
    {

        private Label name = new Label { Text = "" };
        private Label category = new Label { Text = "" };
        private Label refNum = new Label { Text = "" };
        private Label sourceDate = new Label { Text = "" };
        private Label address = new Label { Text = "" };
        private Label cityState = new Label { Text = "" };
        private Label county = new Label { Text = "" };
        private Label people = new Label { Text = "" };
        StackLayout detailStack = new StackLayout();

        public DetailStack()
        {
            detailStack.Children.Add(name);
            detailStack.Children.Add(category);
            detailStack.Children.Add(refNum);
            detailStack.Children.Add(sourceDate);
            detailStack.Children.Add(address);
            detailStack.Children.Add(cityState);
            detailStack.Children.Add(county);
            detailStack.Children.Add(people);
        }

        public StackLayout PopulateStack(DataPoint currentPoint)
        {
            name.Text = currentPoint.Name;
            category.Text = "Category: " + currentPoint.Category;
            refNum.Text = "Reference Number: " + "#" + currentPoint.RefNum;
            sourceDate.Text = "Date added to register: " + currentPoint.SourceDate;
            address.Text = "Reported Street Address: " + currentPoint.Address;
            cityState.Text = "Location: " + currentPoint.City + ", " + currentPoint.State;
            county.Text = "County: " + currentPoint.County;
            people.Text = "Architects/Builders: " + currentPoint.Architects;

            return detailStack;
        }
    }
}
