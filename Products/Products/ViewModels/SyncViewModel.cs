
namespace Products.ViewModels
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Products.Models;
    using Products.Services;
    using Products.View;
    using Xamarin.Forms;

    public class SyncViewModel : INotifyPropertyChanged
    {
        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Services
        NavigationService navigationService;
        ApiService apiService;
        DialogService dialogService;
        DataService dataService;
        #endregion

        #region Attributes
        string _message;
        bool _isToggled;
        bool _isRunning;
        bool _isEnabled;
        #endregion

        #region Properties

        public string Message
        {
            get
            {
                return _message;
            }
            set
            {
                if (_message != value)
                {
                    _message = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Message)));
                }
            }
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
        public SyncViewModel()
        {
            dataService = new DataService();
            apiService = new ApiService();
            dialogService = new DialogService();
            navigationService = new NavigationService();

            Message = "Press sync button to start!";
            IsEnabled = true;
        }
        #endregion
            
        #region Commands
        public ICommand SyncComand
        {
            get
            {
                return new RelayCommand(Sync);
            }
        }

        async void Sync()
        {
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
            var products = dataService.Get<Product>(false).Where(p => p.PendingToSave).ToList();

            //validamos que haya productos
            if (products.Count == 0)
            {
                await dialogService.ShowMessage(
                    "Error",
                    "There are not products to sync!");
            }

            var urlApi = Application.Current.Resources["URLAPI"].ToString();
            var mainViewModels = MainViewModels.GetInstance();

            foreach (var product in products)
            {
                var response = await apiService.Post(
              urlApi,
             "/api",
             "/Products",
             mainViewModels.Token.TokenType,
             mainViewModels.Token.AccessToken
             , product);

                if (response.IsSuccess)
                {
                    product.PendingToSave = false;
                    dataService.Update(product);
                }
            }

            IsRunning = false;
            IsEnabled = true;

            await dialogService.ShowMessage(
                  "Confirmation",
                  "Data Base sync!!");

            //Volvemos a la pagina principal
            await navigationService.BackOnMaster();
        }
        #endregion

    }
}
