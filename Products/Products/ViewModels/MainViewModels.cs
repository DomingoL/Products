using System;
using System.Collections.Generic;
using System.Text;

namespace Products.ViewModels
{
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Models;
    using Services;

    class MainViewModels
    {
        #region Services
        NavigationService navigationService;
        #endregion

        #region Properties
        public LoginViewModel Login
        {
            get;
            set;
        }
        public CategoriesViewModel Categories
        {
            get;
            set;
        }
        public TokenResponse Token
        {
            get;
            set;
        }
        public ProductsViewModel Products
        {
            get;
            set;
        }
        public NewCategoryViewModel NewCategory
        {
            get;
            set;
        }
        #endregion

        #region Constructors
        public MainViewModels()
        {
            navigationService = new NavigationService();
            instance = this;
            Login = new LoginViewModel();
        }
        #endregion

        #region Singleton
        static MainViewModels instance;
        public static MainViewModels GetInstance()
        {
            if (instance == null)
            {
                return new MainViewModels();
            }
            return instance;
        }
        #endregion

        #region Commands
        public ICommand NewCategoryCommand
        {
            get
            {
                return new RelayCommand(GoNewCategory);
            }
        }

        async void GoNewCategory()
        {
            NewCategory = new NewCategoryViewModel();
            await navigationService.Navigate("NewCategoryView");
        }
        #endregion

    }
}
