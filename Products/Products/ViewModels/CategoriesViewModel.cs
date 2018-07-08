
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
            dialogService = new DialogService();
            apiService = new ApiService();
            LoadCategories();
        }
        #endregion

        #region Methods
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

            var categories = (List<Category>)response.Result;
            CategoriesList = new ObservableCollection<Category>(categories.OrderBy(c => c.Description));
        }
        #endregion
    }
}
