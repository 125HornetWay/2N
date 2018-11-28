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
		public Grid Model;
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
				GameGrid.Children.Add(TileElement, Model.GetColumn(i), Model.GetRow(i));
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



			RootEl.Children.Add(Background);
			RootEl.Children.Add(label);

			return RootEl;
		}

		private void BackButton_Clicked(object sender, EventArgs e)
		{
			App.Current.MainPage = new StartPage();
		}

		private void MoveTiles(List<Move> moves)
		{
			Debug.WriteLine(Model.Serialize());
			Debug.WriteLine(Model.ToString());
			/*foreach (Move move in moves)
			{

				if (move.GetType() == typeof(Shift))
				{
					Shift sMove = move as Shift;

					int cY = Model.GetColumn(sMove.i);
					int cX = Model.GetRow(sMove.i);

					int nY = Model.GetColumn(sMove.newIndex);
					int nX = Model.GetRow(sMove.newIndex);
					int nI = Model.GetIndex(nX, nY);

					Xamarin.Forms.Grid.SetColumn(GameGrid.Children[sMove.i], nX);
					Xamarin.Forms.Grid.SetRow(GameGrid.Children[sMove.i], nY);

					StackLayout el = (StackLayout)GameGrid.Children[nI];
					Label lbl = (Label)el.Children[1];
					lbl.Text = Convert.ToString(sMove.tile.Value);
				}
				else if (move.GetType() == typeof(Spawn))
				{

					StackLayout el = (StackLayout)GameGrid.Children[move.i];
					Label lbl = (Label)el.Children[1];
					lbl.Text = Convert.ToString(move.tile.Value);
				} else
				{
					// Combination
					throw new NotImplementedException();
				}
				
			}*/
		}

		private void MoveDown(object sender, EventArgs e)
		{
			MoveTiles(Model.ShiftTiles(Direction.Down));
		}

		private void MoveUp(object sender, EventArgs e)
		{
			MoveTiles(Model.ShiftTiles(Direction.Up));
		}

		private void MoveLeft(object sender, EventArgs e)
		{
			MoveTiles(Model.ShiftTiles(Direction.Left));
		}

		private void MoveRight(object sender, EventArgs e)
		{
			MoveTiles(Model.ShiftTiles(Direction.Right));
		}
	}
}
