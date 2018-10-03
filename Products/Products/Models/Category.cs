
namespace Products.Models
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Xamarin.Forms;
    using ViewModels;
    using Services;
    using System.Threading.Tasks;
    using SQLite.Net.Attributes;
    using SQLiteNetExtensions.Attributes;

    public class Category
    {
        #region Services
        DialogService dialogService;
        NavigationService navigationService;
        #endregion

        #region Properties
        [PrimaryKey]
        public int CategoryId { get; set; }

        public string Description { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Product> Products
        {
            get;
            set;
        }
        #endregion

        #region Contructors
        public Category()
        {
            dialogService = new DialogService();
            navigationService = new NavigationService();
        }
        #endregion

        #region Commands
        public ICommand EditCommand
        {
            get
            {
                return new RelayCommand(Edit);
            }
        }

        public ICommand DeleteCommand
        {
            get
            {
                return new RelayCommand(Delete);
            }
        }
        #endregion

        #region Methods
        async void Delete()
        {
            var response = await dialogService.ShowConfirm(
                        "Confirm",
                     "Are you sure to delete this record?");
            if (!response)
            {
                return;
            }


            //llamamos al CategoriesViewModel, instanciamos por el singleton,llamamos al metodo de delete, pasandole el objeto actual
            CategoriesViewModel.GetInstance().DeleteCategory(this);


        }

        async void Edit()
        {
            MainViewModels.GetInstance().EditCategory = new EditCategoryViewModel(this);
            await navigationService.NavigateOnMaster("EditCategoryView");
        }

        public ICommand SelectCategoryCommand
        {
            get
            {
                return new RelayCommand(SelectCategory);
            }
        }

        async void SelectCategory()
        {
            var mainViewModel = MainViewModels.GetInstance();
            //Mantenemos en memoria la category seleccionada
            mainViewModel.Category = this;
            mainViewModel.Products = new ProductsViewModel(Products);
            await navigationService.NavigateOnMaster("ProductsView");
        }

        public override int GetHashCode()
        {
            return CategoryId;
        }
        #endregion
    }
}
