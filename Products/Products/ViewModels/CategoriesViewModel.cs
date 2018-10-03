
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
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using System.Threading.Tasks;

    public class CategoriesViewModel : INotifyPropertyChanged
    {

        #region Methods
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Attributes
        String _filter;
        List<Category> categories;
        ObservableCollection<Category> _categories;
        bool _isRefreshing;
        #endregion

        #region Services
        ApiService apiService;
        DialogService dialogService;
        DataService dataService;
        #endregion

        #region Properties
        public string Filter
        {
            get
            {
                return _filter;
            }
            set
            {
                if (_filter != value)
                {
                    _filter = value;
                    Search();
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(Filter)));
                }
            }
        }


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
        public bool IsRefreshing
        {
            get
            {
                return _isRefreshing;
            }
            set
            {
                if (_isRefreshing != value)
                {
                    _isRefreshing = value;
                    PropertyChanged?.Invoke(
                        this,
                        new PropertyChangedEventArgs(nameof(IsRefreshing))
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
            dataService = new DataService();
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

        public void AddCategory(Category category)
        {
            IsRefreshing = true;
            categories.Add(category);
            CategoriesList = new ObservableCollection<Category>(categories.OrderBy(c => c.Description));
            IsRefreshing = false;
        }

        public void UpdateCategory(Category category)
        {
            IsRefreshing = true;

            //buscamos la vieja categoria, por el ID
            var oldCategory = categories.Where(c => c.CategoryId == category.CategoryId).FirstOrDefault();
            oldCategory = category;

            CategoriesList = new ObservableCollection<Category>(categories.OrderBy(c => c.Description));
            IsRefreshing = false;
        }

        public async Task DeleteCategory(Category category)
        {
            IsRefreshing = true;

            //buscamos la vieja categoria, por el ID

            var connection = await apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                IsRefreshing = false;
                await dialogService.ShowMessage(
                    "Error",
                    connection.Message);
                return;
            }

            var mainViewModels = MainViewModels.GetInstance();

            var response = await apiService.Delete(
                "http://productsapiis.azurewebsites.net",
                "/api",
                "/Categories",
                mainViewModels.Token.TokenType,
                mainViewModels.Token.AccessToken
                ,category);

            if (!response.IsSuccess)
            {
                IsRefreshing = false;
                await dialogService.ShowMessage(
                    "Error",
                    response.Message);
                return;
            }

            //Eliminamos el objeto de la lista de categorias
            categories.Remove(category);

            CategoriesList = new ObservableCollection<Category>(categories.OrderBy(c => c.Description));
            IsRefreshing = false;
        }

        async void LoadCategories()
        {
            IsRefreshing = true;
            var connetion = await apiService.CheckConnection();
            if (!connetion.IsSuccess)
            {
                categories = dataService.Get<Category>(true);
                if (categories.Count == 0){
                    await dialogService.ShowMessage(
                       "Error",
                       "No cuenta con internet y No hay ninguna categoria cargada.");
                    return;
                }
            }
            else
            {
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
                SaveCategoriesOnDB();
            }


            // CategoriesList = new ObservableCollection<Category>(categories.OrderBy(c => c.Description));
            Search();
            IsRefreshing = false;
        }
        void SaveCategoriesOnDB()
        {
            dataService.DeleteAll<Category>();
            foreach (var category in categories)
            {
                dataService.Insert(category);
                dataService.Save(category.Products);
            }

        }

        #endregion

        #region Commands
        public ICommand SearchCommand
        {
            get
            {
                return new RelayCommand(Search);
            }
        }

        void Search()
        {
            IsRefreshing = true;

            if (string.IsNullOrEmpty(Filter))
            {
                CategoriesList = new ObservableCollection<Category>(
                    categories.OrderBy(c => c.Description));
            }
            else
            {
                CategoriesList = new ObservableCollection<Category>(categories
                    .Where(c => c.Description.ToLower().Contains(Filter.ToLower()))
                    .OrderBy(c => c.Description));
            }

            IsRefreshing = false;
        }

        public ICommand RefreshCommand
        {
            get
            {
                return new RelayCommand(LoadCategories);
            }
        }

        #endregion
    }
}
