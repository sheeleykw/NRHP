﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NRHP_App
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterPage : MasterDetailPage
    {
        public MasterPage(NavigationPage navPage)
        {
            InitializeComponent();
            Detail = navPage;
            //MasterPage.ListView.ItemSelected += ListView_ItemSelected;
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            //var item = e.SelectedItem as MasterPageMenuItem;
            //if (item == null)
            //    return;

            //var page = (Page)Activator.CreateInstance(item.TargetType);
            //page.Title = item.Title;

            //Detail = new NavigationPage(page);
            //IsPresented = false;

            //MasterPage.ListView.SelectedItem = null;
        }
    }
}