using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TWON.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace TWON.View.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class GamePage : ContentPage
	{
		public static Grid Model;
		public List<StackLayout> Mylabels = new List<StackLayout>();
		public GamePage()
		{
			InitializeComponent();

			Model = new Grid();

			Model.PlaceTile();

			Debug.WriteLine(Model.Serialize());
			Debug.WriteLine(Model.ToString());

			int i = 0;  // Yes I know this is convoluted
			foreach (Tile tile in Model.Tiles)
			{
				StackLayout TileElement = CreateTile(tile.Value, tile.GetColor());
				if (tile.Value == 0) TileElement.IsVisible = false;
				GameGrid.Children.Add(TileElement, Model.GetColumn(i), Model.GetRow(i));
				Mylabels.Add(TileElement);
				i++;
			}

			Device.StartTimer(TimeSpan.FromSeconds(1), Model.UpdateTimer);

			Model.UpdateTimeEvent += TimeUpdate;

		}

		void TimeUpdate(object sender, object value)
		{
			TimeLabel.Text = Model.Time.ToString("g");
		}


		public StackLayout CreateTile (int value, Color color)
		{
			var RootEl = new StackLayout();

			var Background = new BoxView();

			Background.WidthRequest = 50;
			Background.HeightRequest = 50;
			Background.BackgroundColor = color;

			var label = new Label
			{
				Text = Convert.ToString(value),
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center,
				FontSize = 20,
				TranslationY = -45
			};

			var EditBox = new Entry
			{
				Placeholder = Convert.ToString(value),
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center,
				FontSize = 20,
				TranslationY = -45,
				IsVisible = false
			};


			RootEl.Children.Add(Background);
			RootEl.Children.Add(label);
			RootEl.Children.Add(EditBox);

			TapGestureRecognizer tap = new TapGestureRecognizer();
			tap.NumberOfTapsRequired = 1;
			tap.CommandParameter = RootEl;
			tap.Command = new Command((sender) =>
			{
				StackLayout root = sender as StackLayout;

				root.Children[1].IsVisible = false;
				root.Children[2].IsVisible = true;
			});

			RootEl.GestureRecognizers.Add(tap);
			EditBox.Completed += Tile_Edited;

			return RootEl;
			//what if I save each of these layouts in a list then access them by index.
		}

		private void Tile_Edited(object sender, EventArgs e)
		{
			Entry entry = sender as Entry;
			Layout root = entry.Parent as Layout;
			Label lbl = root.Children[1] as Label;

			int x = Xamarin.Forms.Grid.GetColumn((StackLayout)root);
			int y = Xamarin.Forms.Grid.GetRow((StackLayout)root);
			int i = Model.GetIndex(x, y);

			try
			{
				lbl.Text = entry.Text;
				Model.Tiles[i].Value = Convert.ToInt32(entry.Text);
			} catch
			{

			}
			

			entry.IsVisible = false;
			lbl.IsVisible = true;

			Debug.WriteLine(Model.ToString());
		}

		private void BackButton_Clicked(object sender, EventArgs e)
		{
			App.Current.MainPage = new StartPage();
		}

		private void MoveTiles(List<Move> moves)
		{
			Debug.WriteLine(Model.Serialize());
			Debug.WriteLine(Model.ToString());
			foreach (Move move in moves)
			{

				if (move.GetType() == typeof(Shift))
				{
					Shift sMove = move as Shift;

					// Update element value
					StackLayout el = (StackLayout)GameGrid.Children[sMove.newIndex];
					Label lbl = (Label)el.Children[1];
					lbl.Text = Convert.ToString(sMove.tile.Value);

					el = (StackLayout)GameGrid.Children[sMove.i];
					lbl = (Label)el.Children[1];
					lbl.Text = Convert.ToString(0);

					GameGrid.Children[sMove.newIndex].IsVisible = true;
					GameGrid.Children[sMove.i].IsVisible = false;

					// Get current coords
					/*int cX = Model.GetColumn(sMove.i);
					int cY = Model.GetRow(sMove.i);

					// Get new coords
					int nX = Model.GetColumn(sMove.newIndex);
					int nY = Model.GetRow(sMove.newIndex);
					int nI = sMove.newIndex;

					GameGrid.Children[sMove.i].IsVisible = true;

					// Update element value
					StackLayout el = (StackLayout)GameGrid.Children[sMove.i];
					Label lbl = (Label)el.Children[1];
					lbl.Text = Convert.ToString(sMove.tile.Value);

					// Need to move old piece somewhere so the indexes don't get rearranged

					// Set coords of current piece
					Xamarin.Forms.Grid.SetRow(GameGrid.Children[sMove.i], nY);
					Xamarin.Forms.Grid.SetColumn(GameGrid.Children[sMove.i], nX);*/
				}
				else if (move.GetType() == typeof(Spawn))
				{

					StackLayout el = (StackLayout)GameGrid.Children[move.i];
					el.IsVisible = true;
					Label lbl = (Label)el.Children[1];
					lbl.Text = Convert.ToString(move.tile.Value);
				} else
				{
					// Combination
					Combination sMove = move as Combination;

					// Update element value
					StackLayout el = (StackLayout)GameGrid.Children[sMove.combinedIndex];
					Label lbl = (Label)el.Children[1];
					lbl.Text = Convert.ToString(sMove.tile.Value);

					el = (StackLayout)GameGrid.Children[sMove.i];
					lbl = (Label)el.Children[1];
					lbl.Text = Convert.ToString(0);

					GameGrid.Children[sMove.combinedIndex].IsVisible = true;
					GameGrid.Children[sMove.i].IsVisible = false;
				}
				
			}
		}

		private void MoveDown(object sender, EventArgs e)
		{
			MoveTiles(Model.ShiftTiles(Direction.Down));
			ScoreLabel.Text = Convert.ToString(Scores.GetScore());
			for (int item = 0; item < Mylabels.Count; ++item)
			{
				Mylabels[item].Children[1].RotateTo(360, 3000);
				Mylabels[item].Children[0].RotateTo(360, 4000);
				Mylabels[item].Children[2].RotateTo(360, 6000);

			}

		}

		private void MoveUp(object sender, EventArgs e)
		{
			MoveTiles(Model.ShiftTiles(Direction.Up));
			MoveTiles(Model.ShiftTiles(Direction.Down));
			ScoreLabel.Text = Convert.ToString(Scores.GetScore());
			for (int item = 0; item < Mylabels.Count; ++item)
			{
				Mylabels[item].Children[1].RotateTo(360, 3000);
				Mylabels[item].Children[0].RotateTo(360, 4000);
				Mylabels[item].Children[2].RotateTo(360, 6000);
			}
		}

		private void MoveLeft(object sender, EventArgs e)
		{
			MoveTiles(Model.ShiftTiles(Direction.Left));
			MoveTiles(Model.ShiftTiles(Direction.Down));
			ScoreLabel.Text = Convert.ToString(Scores.GetScore());
			for (int item = 0; item < Mylabels.Count; ++item)
			{
				Mylabels[item].Children[1].RotateTo(360, 3000);
				Mylabels[item].Children[0].RotateTo(360, 4000);
				Mylabels[item].Children[2].RotateTo(360, 6000);

			}
		}

		private void MoveRight(object sender, EventArgs e)
		{
			MoveTiles(Model.ShiftTiles(Direction.Right));
			MoveTiles(Model.ShiftTiles(Direction.Down));
			ScoreLabel.Text = Convert.ToString(Scores.GetScore());
			for (int item = 0; item < Mylabels.Count; ++item)
			{
				Mylabels[item].Children[1].RotateTo(360, 3000);
				Mylabels[item].Children[0].RotateTo(360, 4000);
				Mylabels[item].Children[2].RotateTo(360, 6000);

			}
		}

		private void Highestscores_Clicked(object sender, EventArgs e)
		{
			App.Current.MainPage = new HighScoreScreen();
		}

	

		private void pause_Clicked(object sender, EventArgs e)
		{
			//do stuff
		}
	}
}
