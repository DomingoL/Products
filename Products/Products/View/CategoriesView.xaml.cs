

namespace Products.View
{
    using System;
    using System.Collections.Generic;
    using Xamarin.Forms;
    using Models;
    using System.Collections.ObjectModel;

    public partial class CategoriesView : ContentPage
    {

        #region Properties
        public ObservableCollection<Category> Categories
        {
            get;
            set;
        }
        #endregion

        #region Constructors
        public CategoriesView()
        {
            LoadCategories();
        }
        #endregion

        #region Methods
        void LoadCategories()
        {
            
        }
        #endregion
    }
}
