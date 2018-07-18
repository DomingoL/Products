using System;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Products.Services;
using Products.ViewModels;

namespace Products.Models
{
    public class Menu
    {
        NavigationService navigationService;

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
                    navigationService.SetMainPage(PageName);
                    break;
                case "UbicationsView":
                    MainViewModels.GetInstance().Ubications =
                        new UbicationsViewModel();
                    await navigationService.NavigateOnMaster(PageName);
                    break;
            }
        }
        #endregion

    }
}
