using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace Products

{
    using View;

	public partial class App : Application
	{
        public static NavigationPage Navigatior { get; internal set; }
        public static MasterView Master { get; internal set; }

        public App ()
		{
			InitializeComponent();

			MainPage = new NavigationPage(new LoginView());

		}

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
