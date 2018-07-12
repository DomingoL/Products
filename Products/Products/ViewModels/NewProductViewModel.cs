using System;
namespace Products.ViewModels
{
    using System;
    using System.ComponentModel;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Products.Models;
    using Products.Services;

    public class NewProductViewModel: INotifyPropertyChanged
    {
        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Services
        NavigationService navigationService;
        ApiService apiService;
        DialogService dialogService;
        #endregion

        #region Attributes
        bool _isRunning;
        bool _isEnabled;
        #endregion

        #region Properties
        public string Description
        {
            get;
            set;
        }

        public string Price
        {
            get;
            set;
        }

        public bool IsActive
        {
            get;
            set;
        }

        public DateTime LastPurchase
        {
            get;
            set;
        }

        public string Stock
        {
            get;
            set;
        }

        public string Remarks
        {
            get;
            set;
        }

        public string Image
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

        
        #region Constructors
        public NewProductViewModel()
        {
            apiService = new ApiService();
            dialogService = new DialogService();
            navigationService = new NavigationService();

            Image = "noimage";
            IsActive = true;
            LastPurchase = DateTime.Today;

            IsEnabled = true;
        }
        #endregion

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
                    "You must enter an Product.");
                return;
            }

            if (string.IsNullOrEmpty(Price))
            {
                await dialogService.ShowMessage(
                "Error",
                    "You must enter an Price.");
                return;
            }
            var price = decimal.Parse(Price);
            if (price <= 0)
            {
                await dialogService.ShowMessage(
                "Error",
                    "You must enter aun Price > 0");
            }

            if (string.IsNullOrEmpty(Stock))
            {
                await dialogService.ShowMessage(
                "Error",
                    "You must enter an Stock.");
                return;
            }
            var stock = Double.Parse(Price);
            if (stock <= 0)
            {
                await dialogService.ShowMessage(
                "Error",
                "You must enter aun Stock > 0");
            }

            if (string.IsNullOrEmpty(Stock))
            {
                await dialogService.ShowMessage(
                "Error",
                    "You must enter an Stock.");
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

            var mainViewModels = MainViewModels.GetInstance();
            var product = new Product
            {
                CategoryId = mainViewModels.Category.CategoryId,
                Description = Description,
                IsActive = IsActive,
                LastPurchase = LastPurchase,
                Price = price,
                Remarks = Remarks,
                Stock = stock,
            };


            var response = await apiService.Post(
                "http://productsapiis.azurewebsites.net",
                "/api",
                "/Products",
                mainViewModels.Token.TokenType,
                mainViewModels.Token.AccessToken
                , product);
            
            if (!response.IsSuccess)
            {
                IsRunning = false;
                IsEnabled = true;
                await dialogService.ShowMessage(
                    "Error",
                    response.Message);
                return;
            }

            product = (Product)response.Result;
            var productsViewModel = ProductsViewModel.GetInstance();
            productsViewModel.Add(product);

            await navigationService.Back();

            IsRunning = false;
            IsEnabled = true;

        }
        #endregion
    }
}
