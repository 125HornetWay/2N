using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TWON.View.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DifficultyScreen : ContentPage
	{
		public DifficultyScreen ()
		{
			InitializeComponent ();
		}

		private void Level2_Clicked(object sender, EventArgs e)
		{
			App.Current.MainPage = new GamePage();
		}

		private void Level3_Clicked(object sender, EventArgs e)
		{
			App.Current.MainPage = new GamePage();
		}

		private void Level1_Clicked(object sender, EventArgs e)
		{
			App.Current.MainPage = new GamePage();
		}

		

		private void Back_Clicked(object sender, EventArgs e)
		{
			App.Current.MainPage = new StartPage();
		}
	}
	}

