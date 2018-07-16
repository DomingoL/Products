

namespace Products.ViewModels
{
    using System;
    using System.ComponentModel;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Products.Services;
    using Models;

    public class EditCategoryViewModel : INotifyPropertyChanged
    {
        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Services
        ApiService apiService;
        NavigationService navigationService;
        DialogService dialogService;
        #endregion

        #region Attributes
        bool _isRunning;
        bool _isEnabled;
        Category category;
        #endregion

        #region Properties
        public string Description
        {
            get;
            set;
        }


        public bool IsRunning
        {
            get
            {
                return _isRunning;
            }
            set
            {
                if (_isRunning != value)
                {
                    _isRunning = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsRunning)));
                }
            }
        }
        public bool IsEnabled
        {
            get
            {
                return _isEnabled;
            }
            set
            {
                if (_isEnabled != value)
                {
                    _isEnabled = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsEnabled)));
                }
            }
        }
        #endregion


        public EditCategoryViewModel(Category category)
        {
            this.category = category;

            apiService = new ApiService();
            dialogService = new DialogService();
            navigationService = new NavigationService();
            //pintamos la Descripción con el la descripción del objeto que recibe
            Description = category.Description;
            IsEnabled = true;

        }

        #region Commands
        public ICommand SaveCommand
        {
            get
            {
                return new RelayCommand(Save);
            }
        }

        async void Save()
        {
            if (string.IsNullOrEmpty(Description))
            {
                await dialogService.ShowMessage(
                "Error",
                    "You must enter an Category.");
                return;
            }

            IsRunning = true;
            IsEnabled = false;

            var connection = await apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                IsRunning = false;
                IsEnabled = true;
                await dialogService.ShowMessage(
                    "Error",
                    connection.Message);
                return;
            }

            //Asignamos la nueva descripción al objeto, con lo que el usuario digitó
            category.Description = Description;

            var mainViewModels = MainViewModels.GetInstance();

            var response = await apiService.Put(
                "http://productsapiis.azurewebsites.net",
                "/api",
                "/Categories",
                mainViewModels.Token.TokenType,
                mainViewModels.Token.AccessToken
                ,category);

            if (!response.IsSuccess)
            {
                IsRunning = false;
                IsEnabled = true;
                await dialogService.ShowMessage(
                    "Error",
                    response.Message);
                return;
            }

            var categoriesViewModel = CategoriesViewModel.GetInstance();
            //actualizamos la categoria.
            categoriesViewModel.UpdateCategory(category);

            await navigationService.BackOnMaster();

            IsRunning = false;
            IsEnabled = true;

        }
        #endregion

    }
}
