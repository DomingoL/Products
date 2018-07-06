using System;
using System.Collections.Generic;
using System.Text;

namespace Products.ViewModels
{
    class MainViewModels
    {
        #region Properties
        public LoginViewModel Login { get; set; }

        #endregion

        #region Constructors
        public MainViewModels()
        {
            Login = new LoginViewModel();
        }
        #endregion

    }
}
