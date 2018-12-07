using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TWON.Model;
using TWON.View.Pages;

namespace TWON.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HighScoreScreen : ContentPage
	{
		List<string> trial_again = new List<string>();
		public HighScoreScreen()
		{	
			InitializeComponent();
			//Generate_HighScoreScreen(trial_again);
		}

		//Generates the highscore screen.
		public void Generate_HighScoreScreen()
		{
			List<string> HighScoreData = HightScoreCollection.Load();
			
			
			int Rank = 1;

			foreach (string line in HighScoreData)
			{
				string[] values = line.Split(' ');
				Ranks.Children.Add(new Label { Text = Rank.ToString() });
				Names.Children.Add(new Label { Text = values[0] });
				Score.Children.Add(new Label { Text = values[1]  });
			}

			++Rank;
		}

		private void HighScores_Clicked(object sender, EventArgs e)
		{
			App.Current.MainPage = new GamePage("N");
		}
	}
}

