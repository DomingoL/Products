
namespace Products.Services
{
    using Xamarin.Forms;
    using System.Threading.Tasks;
    using View;
    using System;

    public class NavigationService
    {
        public void SetMainPage(string pageName)
        {
            switch (pageName)
            {
                case "LoginView":
                    Application.Current.MainPage = new NavigationPage(new LoginView());
                    break;
                case "MasterView":
                    Application.Current.MainPage = new MasterView();
                    break;
            }
        }
        
        public async Task NavigateOnMaster(string pageName)
        {
            App.Master.IsPresented = false;
            
            switch (pageName)
            {
                case "CategoriesView":
                    await App.Navigatior.PushAsync(
                           new CategoriesView());
                    break;
                case "ProductsView":
                    await App.Navigatior.PushAsync(
                        new ProductsView());
                    break;
                case "NewCategoryView":
                    await App.Navigatior.PushAsync(
                        new NewCategoryView());
                    break;
                case "EditCategoryView":
                    await App.Navigatior.PushAsync(
                        new EditCategoryView());
                    break; 
                case "NewProductView":
                    await App.Navigatior.PushAsync(
                        new NewProductView());
                    break; 
                case "EditProductView":
                    await App.Navigatior.PushAsync(
                        new EditProductView());
                    break; 
                case "UbicationsView":
                    await App.Navigatior.PushAsync(
                        new UbicationsView());
                    break; 
                case "SyncView":
                    await App.Navigatior.PushAsync(
                        new SyncView());
                    break; 

            }
        }

        public async Task NavigateOnLogin(string pageName)
        {
            switch (pageName)
            {
                case "NewCustomerView":
                    await Application.Current.MainPage.Navigation.PushAsync(
                        new NewCustomerView());
                    break;
                case "LoginWithFacebook":
                    await Application.Current.MainPage.Navigation.PushAsync(
                        new LoginFacebookView());
                    break;

            }
        }

        public async Task BackOnMaster()
        {
            await App.Navigatior.PopAsync();
        }

        public async Task BackOnLogin()
        {
            await App.Navigatior.PopAsync();
        }
    }
}
