using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TWON.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Plugin.SimpleAudioPlayer;
using System.IO;
using System.Reflection;

namespace TWON.View.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class GamePage : ContentPage
	{
		public static Grid Model;
		public static string SerialisedGame;
		public List<StackLayout> Mylabels = new List<StackLayout>();

		private ISimpleAudioPlayer player;

		Stream GetStreamFromFile(string filename)
		{
			var assembly = typeof(App).GetTypeInfo().Assembly;
			var stream = assembly.GetManifestResourceStream("TWON." + filename);
			return stream;
		}

		public GamePage(string N) : this(DifficultyLevel.Easy, false, N) { }
		public GamePage(DifficultyLevel dl, string N) : this(dl, false, N ) { }
		public GamePage(DifficultyLevel dl, bool cm, string N)
		{

			InitializeComponent();

			//player = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;
			//player.Load("swoosh.mp3");


			int size = 4;
			switch (dl)
			{
				case DifficultyLevel.Medium:
					Model.winningScore = 4096;
					break;
				case DifficultyLevel.Hard:
					size = 6;
					Model.winningScore = 8192;
					break;
				default:
					break;
			}

			Model = new Grid(size, cm);

			Model.PlaceTile();

			Debug.WriteLine(Model.Serialize());
			Debug.WriteLine(Model.ToString());

			GameGrid.RowDefinitions = new RowDefinitionCollection();
			GameGrid.ColumnDefinitions = new ColumnDefinitionCollection();
			GameGrid.Layout(new Rectangle(280, 270, (100 * Model._columns) + (8 * Model._columns), (100 * Model._columns) + (8 * Model._columns)));

			var rows = CreateRows(Model._columns);
			var cols = CreateCols(Model._columns);

			for (int j = 0; j < Model._columns; j++)
			{
				GameGrid.RowDefinitions.Add(rows[j]);
				GameGrid.ColumnDefinitions.Add(cols[j]);
			}

			int i = 0;  // Yes I know this is convoluted

			if (N == "N")
			{
				foreach (Tile tile in Model.Tiles)
				{
					StackLayout TileElement = CreateTile(tile.Value, tile.GetColor());
					if (tile.Value == 0) TileElement.IsVisible = false;
					GameGrid.Children.Add(TileElement, Model.GetColumn(i), Model.GetRow(i));
					Mylabels.Add(TileElement);
					i++;
				}

				Scores.Score = 0;

			}
			else if (N == "C")
			{

				foreach (Tile tile in Grid.SavedGrid)
				{
					StackLayout TileElement = CreateTile(tile.Value, tile.GetColor());
					if (tile.Value == 0) TileElement.IsVisible = false;
					GameGrid.Children.Add(TileElement, Model.GetColumn(i), Model.GetRow(i));
					Mylabels.Add(TileElement);
					i++;
				}

				Scores.Score = Scores.SavedScore;
				ScoreLabel.Text = Scores.Score.ToString();
				Model.Tiles = Grid.SavedGrid;

			}

			Device.StartTimer(TimeSpan.FromSeconds(1), Model.UpdateTimer);

			Model.UpdateTimeEvent += TimeUpdate;

		}

		//The new constructor needs to set the saved game scores.
		//The new constructor needs to set the game tiles to the previous stiles provided.



		void TimeUpdate(object sender, object value)
		{
			TimeLabel.Text = Model.Time.ToString("g");
		}

		public List<RowDefinition> CreateRows(int size)
		{
			List<RowDefinition> rows = new List<RowDefinition>();

			for (int i = 0; i < size; i++)
			{
				RowDefinition row = new RowDefinition
				{
					Height = 100
				};

				rows.Add(row);
			}

			return rows;
		}

		public List<ColumnDefinition> CreateCols(int size)
		{
			List<ColumnDefinition> cols = new List<ColumnDefinition>();

			for (int i = 0; i < size; i++)
			{
				ColumnDefinition col = new ColumnDefinition
				{
					Width = 100
				};

				cols.Add(col);
			}

			return cols;
		}


		public StackLayout CreateTile(int value, Color color)
		{
			var RootEl = new StackLayout();

			var Background = new BoxView();

			Background.WidthRequest = 100;
			Background.HeightRequest = 100;
			Background.BackgroundColor = color;

			var label = new Label
			{
				Text = Convert.ToString(value),
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center,
				FontSize = 40,
				TranslationY = -85
			};

			RootEl.Children.Add(Background);
			RootEl.Children.Add(label);

			if (Model.cheatMode)
			{
				var EditBox = new Entry
				{
					HorizontalOptions = LayoutOptions.Center,
					VerticalOptions = LayoutOptions.Center,
					FontSize = 20,
					TranslationY = -45,
					IsVisible = false
				};

				RootEl.Children.Add(EditBox);

				TapGestureRecognizer tap = new TapGestureRecognizer();
				tap.NumberOfTapsRequired = 1;
				tap.CommandParameter = RootEl;
				tap.Command = new Command((sender) =>
				{
					StackLayout root = sender as StackLayout;

					root.Children[1].IsVisible = true;
					root.Children[2].IsVisible = true;

					root.Children[2].Focus();
				});

				RootEl.GestureRecognizers.Add(tap);
				EditBox.Completed += Tile_Edited;
			}

			return RootEl;
			//what if I save each of these layouts in a list then access them by index.
		}

		private void Tile_Edited(object sender, EventArgs e)
		{
			Entry entry = sender as Entry;
			Layout root = entry.Parent as Layout;
			Label lbl = root.Children[1] as Label;

			int y = Xamarin.Forms.Grid.GetColumn((StackLayout)root);
			int x = Xamarin.Forms.Grid.GetRow((StackLayout)root);
			int i = Model.GetIndex(x, y);

			try
			{
				lbl.Text = entry.Text;
				Model.Tiles[i].Value = Convert.ToInt32(entry.Text);
			}
			catch
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
				}
				else
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
			//player.Play();
			RotatePieces();

		}

		private void MoveUp(object sender, EventArgs e)
		{
			MoveTiles(Model.ShiftTiles(Direction.Up));
			ScoreLabel.Text = Convert.ToString(Scores.GetScore());
			//player.Play();
			RotatePieces();

		}

		private void MoveLeft(object sender, EventArgs e)
		{
			MoveTiles(Model.ShiftTiles(Direction.Left));
			ScoreLabel.Text = Convert.ToString(Scores.GetScore());
			//player.Play();
			RotatePieces();

		}

		private void MoveRight(object sender, EventArgs e)
		{
			MoveTiles(Model.ShiftTiles(Direction.Right));
			ScoreLabel.Text = Convert.ToString(Scores.GetScore());
			//player.Play();
			RotatePieces();
		}

		private void RotatePieces()
		{
			foreach (int item in TWON.Model.Animation.currentcombinations)
			{
				Mylabels[item].Children[1].RotateTo(360, 200);
				Mylabels[item].Children[1].ScaleTo(2, 2000);

			}
		}

		private void Highestscores_Clicked(object sender, EventArgs e)
		{
			App.Current.MainPage = new HighScoreScreen();
		}



		private void pause_Clicked(object sender, EventArgs e)
		{
			
			string SerialisedGame = GamePage.Model.Serialize();
			GamePage.SerialisedGame = SerialisedGame;
			Grid.SavedGrid = Model.Tiles;
			Application.Current.Properties["savedgame"] = SerialisedGame;
			Scores.SavedScore = Scores.Score;

			//There should be a way to save the tiles off so that when the games starts, You just grabe the saved tiles.
		}
	}
}
