using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using TWON.View.Pages;
using TWON;

namespace TWON
{
	public partial class App : Application
	{
		public App()
		{
			MainPage = new StartPage();
		}

		protected override void OnStart()
		{
			// Handle when your app starts
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
			if (GamePage.Model != null)
				Application.Current.Properties["savedgame"] = GamePage.Model.Serialize();
		

		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}
	}
}
