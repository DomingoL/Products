using System;
using System.Collections.Generic;
using System.Text;

namespace Products.ViewModels
{
    using System.Collections.ObjectModel;
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
        public EditCategoryViewModel EditCategory
        {
            get;
            set;
        }
        public Category Category
        {
            get;
            set;
        }
        public NewProductViewModel NewProduct
        {
            get;
            set;
        }
        public EditProductViewModel EditProduct
        {
            get;
            set;
        }
        public NewCustomerViewModel NewCustomer
        {
            get;
            set;
        }
        public ObservableCollection<Menu> MyMenu
        {
            get;
            set;
        }
        public UbicationsViewModel Ubications
        {
            get;
            set;
        }
        public SyncViewModel Sync
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
            LoadMenu();
        }
        #endregion

        #region Methods
        private void LoadMenu()
        {
            MyMenu = new ObservableCollection<Menu>();
            MyMenu.Add(new Menu
            {
                Icon = "ic_settings",
                PageName = "MyProfileView",
                Title = "My Profile",
            });

            MyMenu.Add(new Menu
            {
                Icon = "ic_map",
                PageName = "UbicationsView",
                Title = "Ubications",
            });

            MyMenu.Add(new Menu
            {
                Icon = "ic_sync",
                PageName = "SyncView",
                Title = "Sync Offline Operations",
            });

            MyMenu.Add(new Menu
            {
                Icon = "ic_exit_to_app",
                PageName = "LoginView",
                Title = "Close sesion",
            });


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

        public ICommand NewProductCommand
        {
            get
            {
                return new RelayCommand(GoNewProduct);
            }
        }


        async void GoNewCategory()
        {
            NewCategory = new NewCategoryViewModel();
            await navigationService.NavigateOnMaster("NewCategoryView");
        }

        async void GoNewProduct()
        {
            NewProduct = new NewProductViewModel();
            await navigationService.NavigateOnMaster("NewProductView");
        }
        #endregion

    }
}
