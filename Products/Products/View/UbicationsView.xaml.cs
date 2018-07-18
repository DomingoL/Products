
namespace Products.View
{
    using System.Threading.Tasks;
    using Services;
    using ViewModels;
    using Xamarin.Forms;
    using Xamarin.Forms.Maps;

    public partial class UbicationsView : ContentPage
    {
        #region Services
        GeolocatorService geolocatorService;
        #endregion

        #region Constructors
        public UbicationsView()
        {
            InitializeComponent();

            geolocatorService = new GeolocatorService();

            MoveMapToCurrentPosition();
        }
        #endregion

        #region Methods
        async void MoveMapToCurrentPosition()
        {
            await geolocatorService.GetLocation();
            if (geolocatorService.Latitude != 0 ||
                geolocatorService.Longitude != 0)
            {
                var position = new Position(
                    geolocatorService.Latitude,
                    geolocatorService.Longitude);
                MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(
                    position,
                    Distance.FromKilometers(.5)));
            }


            #endregion
        }
    }
}
