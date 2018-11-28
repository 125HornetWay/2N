using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TWON.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HighScoreScreen : ContentPage
	{
		List<string> trial_again = new List<string>();
		public HighScoreScreen()
		{
			InitializeComponent();
			Generate_HighScoreScreen(trial_again);
		}

		public void Generate_HighScoreScreen(List<string> HighScoreData)
		{
			string first_trial = "1 Dorcas 678";
			string second_trial = "2 Lanyero 4567";
			HighScoreData.Add(first_trial);
			HighScoreData.Add(second_trial);
			foreach(string line in HighScoreData)
			{
				string[] values = line.Split(' ');
				Ranks.Children.Add(new Label { Text = values[0] });
				Names.Children.Add(new Label { Text = values[1] });
				Score.Children.Add(new Label { Text = values[2] });
			}
			
		}
	}
}
