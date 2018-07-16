using System;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Products.Services;

namespace Products.Models
{
    public class Menu
    {
        NavigationService navigationService;

        public string Icon
        {
            get;
            set;
        }
        public string Title
        {
            get;
            set;
        }
        public string PageName
        {
            get;
            set; 
        }
        #region Icommands
        public ICommand NavigateCommand
        {
            get
            {
                return new RelayCommand(Navigate);
            }
        }

        private void Navigate()
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}
