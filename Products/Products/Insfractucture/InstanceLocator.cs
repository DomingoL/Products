
namespace Products.Insfractucture
{
    using Products.ViewModels;
    class InstanceLocator
    {
        public MainViewModels Main { get; set; }

        public InstanceLocator()
        {
            Main = new MainViewModels();
        }
    }
}
