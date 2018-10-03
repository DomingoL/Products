using System;

using Xamarin.Forms;

namespace Products.View
{
    public class SyncView : ContentPage
    {
        public SyncView()
        {
            Content = new StackLayout
            {
                Children = {
                    new Label { Text = "Hello ContentPage" }
                }
            };
        }
    }
}

