using System;
using System.Linq;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Products.Services;
using Products.ViewModels;
using Xamarin.Forms;

namespace Products.Models
{
    public class Menu
    {

        #region Services
        ApiService apiService;
        DialogService dialogService;
        NavigationService navigationService;
        DataService dataService;
        #endregion


        public string Icon
        {
            get;
            set;
        }
        public string Title
        {
            get;
            set;
        }
        public string PageName
        {
            get;
            set; 
        }
        #region Constructors
        public Menu()
        {
            apiService = new ApiService();
            dataService = new DataService();
            dialogService = new DialogService();
            navigationService = new NavigationService();
        }
        #endregion
        #region Icommands
        public ICommand NavigateCommand
        {
            get
            {
                return new RelayCommand(Navigate);
            }
        }

        async void Navigate()
        {
            switch (PageName)
            {
                case "LoginView":
                    var mainViewModel = MainViewModels.GetInstance();
                    mainViewModel.Token.IsRemembered = false;
                    dataService.Update(mainViewModel.Token);
                    mainViewModel.Login = new LoginViewModel();
                    navigationService.SetMainPage(PageName);
                    break;
                case "UbicationsView":
                    MainViewModels.GetInstance().Ubications =
                        new UbicationsViewModel();
                    await navigationService.NavigateOnMaster(PageName);
                    break;
                case "SyncView":
                    MainViewModels.GetInstance().Sync =
                                           new SyncViewModel();
                    await navigationService.NavigateOnMaster(PageName);
                    break;
                case "MyProfileView":
                    MainViewModels.GetInstance().MyProfile =
                                           new MyProfileViewModel();
                    await navigationService.NavigateOnMaster(PageName);
                    break;
            }
        }


        #endregion

    }
}
