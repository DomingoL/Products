using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace Products

{
    using Products.Models;
    using Products.Services;
    using Products.ViewModels;
    using View;

	public partial class App : Application
	{
        #region Services
        ApiService apiService;
        DialogService dialogService;
        NavigationService navigationService;
        DataService dataService;
        #endregion

        #region Properties
        public static NavigationPage Navigatior { get; internal set; }
        public static MasterView Master { get; internal set; }
        #endregion

        #region Constructors
        public App()
        {
            apiService = new ApiService();
            dialogService = new DialogService();
            navigationService = new NavigationService();
            dataService = new DataService();

            InitializeComponent();

            var token = dataService.First<TokenResponse>(false);
            if (token != null &&
                token.IsRemembered &&
                token.Expires > DateTime.Now)
            {
                var mainViewModel = MainViewModels.GetInstance();
                mainViewModel.Token = token;
                mainViewModel.Categories = new CategoriesViewModel();
                navigationService.SetMainPage("MasterView");
            }
            else
            {
                navigationService.SetMainPage("LoginView");
            }

        }
        #endregion

        #region Methods
        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        public static Action LoginFacebookFail
        {
            get
            {
                return new Action(() => Current.MainPage =
                                  new NavigationPage(new LoginView()));
            }
        }

        public async static void LoginFacebookSuccess(FacebookResponse profile)
        {
            if (profile == null)
            {
                Current.MainPage = new NavigationPage(new LoginView());
                return;
            }

            var apiService = new ApiService();
            var dialogService = new DialogService();

            var checkConnetion = await apiService.CheckConnection();
            if (!checkConnetion.IsSuccess)
            {
                await dialogService.ShowMessage("Error", checkConnetion.Message);
                return;
            }

            var urlAPI = Application.Current.Resources["URLAPI"].ToString();
            var token = await apiService.LoginFacebook(
                urlAPI,
                "/api",
                "/Customers/LoginFacebook",
                profile);

            if (token == null)
            {
                await dialogService.ShowMessage(
                    "Error",
                    "Problem ocurred retrieving user information, try latter.");
                Current.MainPage = new NavigationPage(new LoginView());
                return;
            }

            var mainViewModel = MainViewModels.GetInstance();
            mainViewModel.Token = token;
            mainViewModel.Categories = new CategoriesViewModel();
            Current.MainPage = new MasterView();
        }
        #endregion
	}
}
