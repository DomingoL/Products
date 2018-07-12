

namespace Products.Models
{
    using System;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Products.Services;
    using Products.ViewModels;

    public class Product
    {
        #region Services
        DialogService dialogService;
        NavigationService navigationService;
        #endregion

        public int ProductId { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }

        public string Description { get; set; }

        public string Image { get; set; }

        public decimal Price { get; set; }

        public bool IsActive { get; set; }

        public DateTime LastPurchase { get; set; }

        public double Stock { get; set; }

        public string Remarks { get; set; }

        public string ImageFullPath
        {
            get
            {
                if (string.IsNullOrEmpty(Image))
                {
                    return "noimage";
                }

                return string.Format("http://productsbackend.azurewebsites.net/{0}",
                                     Image.Substring(1));
            }
        }

        #region Contructors
        public Product()
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
            ProductsViewModel.GetInstance().Delete(this);


        }

        async void Edit()
        {
            MainViewModels.GetInstance().EditProductView = new EditProductViewModel(this);
            await navigationService.Navigate("EditProductView");
        }
        #endregion

    }
}
