using System;
using System.Collections.Generic;
using System.Text;

namespace Products.ViewModels
{
    using Models;
    class MainViewModels
    {
        #region Properties
        public LoginViewModel Login
        {
            get;
            set;
        }
        public CategoriesViewModel Categories
        {
            get;
            set;
        }
        public TokenResponse Token
        {
            get;
            set;
        }
        #endregion

        #region Constructors
        public MainViewModels()
        {
            instance = this;
            Login = new LoginViewModel();
        }
        #endregion

        #region Singleton
        static MainViewModels instance;
        public static MainViewModels GetInstance()
        {
            if (instance == null)
            {
                return new MainViewModels();
            }
            return instance;
        }
        #endregion

    }
}
