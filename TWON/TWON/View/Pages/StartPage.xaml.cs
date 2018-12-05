using System;
using Plugin.SimpleAudioPlayer;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SkiaSharpFormsDemos;

using TWON.View.Pages;

namespace TWON.View.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class StartPage : ContentPage
		
	{
		public StartPage()
		{

			InitializeComponent ();
			
			var player = CrossSimpleAudioPlayer.Current;
            player.Load("LionKing.mp3");
            player.Play();

        }

		
		private void About_Clicked(object sender, EventArgs e)
		{
			App.Current.MainPage = new NavigationPage(new HomePage());

		}

		private void Difficulty_Clicked(object sender, EventArgs e)
		{
			App.Current.MainPage = new DifficultyScreen();

		}

		private void Help_Clicked(object sender, EventArgs e)
		{
			App.Current.MainPage = new HelpScreen();

		}

		private void Cheat_Clicked(object sender, EventArgs e)
		{
			//Implemet cheat 

			InitializeComponent();
		}

		private void OnStartTapped(object sender, EventArgs e)
		{
			App.Current.MainPage = new GamePage();

		}



		private void Continue_Clicked(object sender, EventArgs e)
		{
			Grid grid = Grid.Deserialize(GamePage.SerialisedGame);
			//GamePage.Model.Tiles = GamePage.Model.
			//Ap//p.Current.MainPage = new GamePage();

			GamePage.Model = grid;
			GamePage newpage = new GamePage();
			newpage.GamePage2();
		}
	}
}
;
