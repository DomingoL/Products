using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Products.View
{
    public partial class MasterView : MasterDetailPage
    {
        public MasterView()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.Navigatior = Navigator;
            App.Master = this;
        }
    }
}
