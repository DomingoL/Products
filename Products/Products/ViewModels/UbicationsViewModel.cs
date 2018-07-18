using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Products.Services;
using Xamarin.Forms.Maps;

namespace Products.ViewModels
{
    using System.Collections.Generic;
    using Models;
    public class UbicationsViewModel
    {
        #region Properties
        public ObservableCollection<Pin> Pins
        {
            get;
            set;
        }
        #endregion
        #region Services
        DialogService dialogService;
        ApiService apiService;
        #endregion

        #region Constructors
        public UbicationsViewModel()
        {
            instance = this;
            dialogService = new DialogService();
            apiService = new ApiService();

        }
        #endregion

        #region Methods
        public async Task LoadPines()
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

                var response = await apiService.GetList<Ubication>(
                   "http://productsapiis.azurewebsites.net",
                   "/api",
                    "/Ubications",
                    mainViewModel.Token.TokenType,
                    mainViewModel.Token.AccessToken);

                if (!response.IsSuccess)
                {
                    await dialogService.ShowMessage(
                        "Error",
                        response.Message);
                    return;
                }

                var ubications = (List<Ubication>)response.Result;
            Pins = new ObservableCollection<Pin>();
             foreach (var ubication in ubications)
            {
                Pins.Add(new Pin
                {
                    Address = ubication.Address,
                    Label = ubication.Description,
                    Position = new Position(ubication.Latitude, ubication.Longitude),
                    Type = PinType.Place,
                });
            }
        }
        #endregion

        #region Singleton
        static UbicationsViewModel instance;
        public static UbicationsViewModel GetInstance()
        {
            if (instance == null)
            {
                return new UbicationsViewModel();
            }
            return instance;
        }
        #endregion
    }
}
