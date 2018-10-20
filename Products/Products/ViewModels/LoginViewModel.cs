
namespace Products.ViewModels
{
    using System;
    using System.ComponentModel;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Products.Services;
    using Products.View;
    using Xamarin.Forms;

    class LoginViewModel : INotifyPropertyChanged
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
        string _email;
        string _password;
        bool _isToggled;
        bool _isRunning;
        bool _isEnabled;
        #endregion

        #region Properties
        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                if (_email != value)
                {
                    _email = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Email)));
                }
            }
        }
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                if (_password != value)
                {
                    _password = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Password)));
                }
            }
        }
        public bool IsToggled
        {
            get
            {
                return _isToggled;
            }
            set
            {
                if (_isToggled != value)
                {
                    _isToggled = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsToggled)));
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
        public LoginViewModel()
        {
            dataService = new DataService();
            apiService = new ApiService();
            dialogService = new DialogService();
            navigationService = new NavigationService();
            IsEnabled = true;
            IsToggled = true;
        }
        #endregion
        #region Commands

        public ICommand LoginComand
        {
            get
            {
                return new RelayCommand(Login);
            }
        }
        public ICommand RegisterNewUserComand
        {
            get
            {
                return new RelayCommand(RegisterNewUser);
            }
        }

        public ICommand LoginWithFacebookComand
        {
            get
            {
                return new RelayCommand(LoginWithFacebook);
            }
        }

        async void LoginWithFacebook()
        {
            await navigationService.NavigateOnLogin("LoginWithFacebook");
        }

        async void RegisterNewUser()
        {
            MainViewModels.GetInstance().NewCustomer = new NewCustomerViewModel();
            await navigationService.NavigateOnLogin("NewCustomerView");
        }

        async void Login()
        {
           if (string.IsNullOrEmpty(Email))
            {
                await dialogService.ShowMessage(
                "Error",
                    "You must enter an email.");
                return;
                }
            if (string.IsNullOrEmpty(Password))
            {
                await dialogService.ShowMessage(
                "Error",
                    "You must enter an password.");
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

            var urlAPI = Application.Current.Resources["URLAPI"].ToString();


            var response = await apiService.GetToken(
                urlAPI,  
                Email, 
                Password);

            if (response == null)
            {
                IsRunning = false;
                IsEnabled = true;
                await dialogService.ShowMessage(
                    "Error",
                    "The service is not available, please try latter.");
                return; 
            }

            if (string.IsNullOrEmpty(response.AccessToken))
            {
                IsRunning = false;
                IsEnabled = true;
                await dialogService.ShowMessage(
                    "Error",
                    response.ErrorDescription);
                return;
            }

            response.IsRemembered = IsToggled;
            response.Password = Password;
            dataService.DeleteAllAndInsert(response);

            var mainViewModel = MainViewModels.GetInstance();
            mainViewModel.Categories = new CategoriesViewModel();
            mainViewModel.Token = response;
            navigationService.SetMainPage("MasterView");

            IsRunning = false;
            IsEnabled = true;
            Email = null;
            Password = null;
        }
        #endregion
    }
}
