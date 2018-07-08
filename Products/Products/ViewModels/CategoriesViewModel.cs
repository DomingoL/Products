
namespace Products.ViewModels
{

    using System;
    using System.Collections.Generic;
    using Xamarin.Forms;
    using Models;
    using System.Collections.ObjectModel;
    using Products.Services;
    using System.ComponentModel;
    using System.Linq;

    public class CategoriesViewModel : INotifyPropertyChanged
    {

        #region Methods
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Attributes
        List<Category> categories;
        ObservableCollection<Category> _categories;
        #endregion

        #region Services
        ApiService apiService;
        DialogService dialogService;
        #endregion

        #region Properties
        public ObservableCollection<Category> CategoriesList
        {
            get
            {
                return _categories;
            }
            set
            {
                if (_categories != value)
                {
                    _categories = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(CategoriesList))
                    );
                }
            }
        }
        #endregion

        #region Constructors
        public CategoriesViewModel()
        {
            instance = this;
            dialogService = new DialogService();
            apiService = new ApiService();
            LoadCategories();
        }
        #endregion


        #region Singleton
        static CategoriesViewModel instance;
        public static CategoriesViewModel GetInstance()
        {
            if (instance == null)
            {
                return new CategoriesViewModel();
            }
            return instance;
        }
        #endregion


        #region Methods
        public void AddCategory(Category category ){
            categories.Add(category);
            CategoriesList = new ObservableCollection<Category>(categories.OrderBy(c => c.Description));
        }

        async void LoadCategories()
        {
            var connetion = await apiService.CheckConnection();
            if (!connetion.IsSuccess)
            {
                await dialogService.ShowMessage(
                    "Error",
                    connetion.Message);
                return;
            }

            var mainViewModel = MainViewModels.GetInstance();

            var response = await apiService.GetList<Category>(
               "http://productsapiis.azurewebsites.net",
               "/api",
                "/Categories",
                mainViewModel.Token.TokenType,
                mainViewModel.Token.AccessToken);
            
            if (!response.IsSuccess)
            {
                await dialogService.ShowMessage(
                    "Error",
                    response.Message);
                return;
            }

            categories = (List<Category>)response.Result;
            CategoriesList = new ObservableCollection<Category>(categories.OrderBy(c => c.Description));
        }
        #endregion
    }
}
