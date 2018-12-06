using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TWON.Model;
using static TWON.Direction;

// File: Grid.cs
// Description: Main model

namespace TWON
{
	// Enum that represents which direction on the grid to move the pieces
	public enum Direction
	{
		Up, Down, Left, Right
	}

	// Enum representing the difficulty level at which to initialize the grid
	public enum DifficultyLevel
	{
		Easy, Medium, Hard
	}

	// Holds all the main data for the game and all of the methods for playing the game
	public class Grid
	{
		public EventHandler<object> UpdateTimeEvent; // Event that fires to update the timer in the view
		public Tile[] Tiles { get; set; } // List of Tiles that for mthe grid
		public bool GameOver = false; // Boolean that represents whether the game is over or not
		public bool cheatMode = false; // Represents whether cheat mode is active
		public static Tile[] SavedGrid; // Holds a deserialized Tile grid

		public int winningScore = 2048; // The tile value that needs to be obtained in order to win the game

		public TimeSpan Time = TimeSpan.Zero; // Holds the amount of time the game has been active

		protected static CryptoRandom rand = new CryptoRandom();  // Random object for placing new tiles

		public readonly int _columns; // number of columns
		private readonly int _gridSize; // number of pieces in the grid

		public bool UpdateTimer() // Adds a second to the timer every second the game is active
		{
			if (!GameOver)
			{
				Time = Time.Add(TimeSpan.FromSeconds(1));
				if (UpdateTimeEvent != null)
					UpdateTimeEvent(this, null);
				return true;
			} else
			{
				return false;
			}
				

		}

		public string Serialize() { // Serialize the model into a string to be saved
			string s = _columns + ";";
			s += Time.ToString() + ";";
			for (int i = 0; i < Tiles.Length; i++)
			{
				s += Tiles[i].Serialize();
				if (i < Tiles.Length - 1)
					s += ",";
			}

			return s;
		}

		public static Grid Deserialize(string s) // Deserialize a model string
		{
			string[] split = s.Split(';');
			Grid grid = new Grid(Convert.ToInt32(split[0]));

			grid.Time = TimeSpan.Parse(split[1]);

			string[] tiles = split[2].Split(',');

			for (int i = 0; i < tiles.Length; i++)
			{
				grid.Tiles[i] = Tile.Deserialize(tiles[i]);
			}

			return grid;
			
		}

		public Grid(int size) // Grid constructor
		{
			_columns = size;
			_gridSize = size * size;
			Tiles = new Tile[_gridSize];

			for (int i = 0; i < _gridSize; ++i)
			{
				Tiles[i] = new Tile();
			}
		}

		public Grid() : this(4) { }

		public Grid(int size, bool cheatMode) : this(size)
		{
			this.cheatMode = cheatMode;
		}

		// row # of item at idx
		public int GetRow(int idx) // return the row of an index
		{
			return idx / _columns;
		}

		// column # of item at idx
		public int GetColumn(int idx) // return the column of an index
		{
			return idx % _columns;
		}

		public int GetIndex(int x, int y) // return the index of a row and column
		{
			return y + x * _columns;
		}

		public Move MoveTile(int i, Direction d) // Move a tile on the grid
		{
			int finalIndex = i;
			bool combination = false;
			bool pieceShifted = true;
			Tile tile = Tiles[i];
			while (pieceShifted && !combination)
			{

				// going up = subtracting _columns
				// going down = adding _columns
				// going left = subtracting one
				// going right = adding one
				int testIndex = Int32.MinValue;
				switch (d)
				{
					case Direction.Down:
						testIndex = finalIndex + _columns;
						break;
					case Direction.Up:
						testIndex = finalIndex - _columns;
						break;
					case Direction.Left:
						testIndex = finalIndex - 1;
						break;
					case Direction.Right:
						testIndex = finalIndex + 1;
						break;
					default:
						throw new System.Exception("Invalid direction");
				}

				try
				{
					var moves = GetDirections()[finalIndex];
					if (GetDirections()[finalIndex].Contains(d))
					{
						if (Tiles[testIndex].Value == 0)
						{
							// If a position is empty
							finalIndex = testIndex;
						}
						else if (Tiles[testIndex].Value == Tiles[i].Value)
						{
							// If the tile can be combined
							finalIndex = testIndex;
							combination = true;
						}
						else
						{
							pieceShifted = false;
						}
					} else
					{
						pieceShifted = false;
					}
				} catch
				{
					pieceShifted = false;
				}
				

				//c++;
			}

			if (finalIndex != i)
			{
				if (combination)
				{
					Tiles[finalIndex].Value = Tiles[i].Value + Tiles[finalIndex].Value;
					Tiles[i].Value = 0;
					Animation.currentcombinations.Add(finalIndex);
					Scores.UpdateScore(Tiles[i].Value + Tiles[finalIndex].Value);
					return new Combination(i, finalIndex, Tiles[finalIndex]);
				} else
				{
					Tiles[finalIndex].Value = Tiles[i].Value;
					Tiles[i].Value = 0;
					return new Shift(i, finalIndex, Tiles[finalIndex]);
				}
				
			} else
			{
				return null;
			}
		}
		

		// use memorization so we don't have to iterate every time we call GetDirections()
		private readonly List<HashSet<Direction>> Dir_Memos = new List<HashSet<Direction>>();

		// Generatea table of valid move directions for each piece on the grid
		public List<HashSet<Direction>> GetDirections()
		{
			if (Dir_Memos.Count == 0)
			{
				for (int i = 0, row, col; i < _gridSize; ++i)
				{
					row = GetRow(i);
					col = GetColumn(i);
					var set = new HashSet<Direction>();

					if (row == 0)
					{
						set.Add(Down);
					}
					else if (row == _columns - 1)
					{
						set.Add(Up);
					}
					else
					{
						set.Add(Up);
						set.Add(Down);
					}

					if (col > 0)
					{
						set.Add(Left);
					}

					if (col < _columns - 1)
					{
						set.Add(Right);
					}

					Dir_Memos.Add(set);
				}
			}

			return Dir_Memos;
		}

		// randomly place one 2 on an empty tile
		public int PlaceTile()
		{
			while (!GameOver)
			{
				int i = rand.Next(_gridSize);
				Tile tile = Tiles[i];

				if (tile.Value == 0)
				{
					tile.Value = 2;
					return i;
				}
			}

			return -1;
		}

		// shift all tile values on the grid in the given direction
		public List<Move> ShiftTiles(Direction dir)
		{
			List<Move> moves = new List<Move>();
			if (!GameOver)
			{
				if (dir == Direction.Down || dir == Direction.Right)
				{
					// Count i down
					for (int i = _gridSize - 1; i >= 0; --i)
					{
						if (GetDirections()[i].Contains(dir) && Tiles[i].Value > 0)
						{
							Move move = MoveTile(i, dir);
							if (move != null)
								moves.Add(move);
						}
					}
				}
				else
				{
					// Count i up
					for (int i = 0; i < _gridSize; ++i)
					{
						if (GetDirections()[i].Contains(dir) && Tiles[i].Value > 0)
						{
							Move move = MoveTile(i, dir);
							if (move != null)
								moves.Add(move);
						}
					}
				}

				if ((Tiles.All(tile => tile.Value > 0) && moves.Count == 0) || Tiles.Any(tile => tile.Value == winningScore))
				{
					GameOver = true;
				}
				else if (moves.Count > 0)
				{
					int newTile = PlaceTile();
					moves.Add(new Spawn(newTile, Tiles[newTile]));
				}
			}

			return moves;
		}

		public override string ToString() // prints Tile grid to the console
		{
			string output = "";

			for (int i = 0; i < Tiles.Length; i++)
			{
				output += Tiles[i].Value;
				for (int j = 0; j < (5 - Convert.ToString(Tiles[i].Value).Length); j++)
				{
					output += " ";
				}
				

				if (i % (_columns) == (_columns - 1))
				{
					output += "\n";
				}
			}

			return output;
		}


	}
}
