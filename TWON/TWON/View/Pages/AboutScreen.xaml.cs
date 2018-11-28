using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TWON.View.Pages;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TWON.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AboutScreen : ContentPage
	{
		public AboutScreen()
		{
			InitializeComponent();
		}

		private void Back_Clicked(object sender, EventArgs e)
		{
			App.Current.MainPage = new StartPage();
		}
	}


	}

