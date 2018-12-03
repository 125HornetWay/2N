using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TWON.Model;
using static TWON.Direction;

namespace TWON
{
	public enum Direction
	{
		Up, Down, Left, Right
	}

	public class Grid
	{
		public EventHandler<object> UpdateTimeEvent;
		public Tile[] Tiles { get; set; }
		public bool GameOver = false;
		public bool cheatMode = false;

		public int winningScore = 2048;

		public TimeSpan Time = TimeSpan.Zero;

		protected static CryptoRandom rand = new CryptoRandom();

		public readonly int _columns;
		private readonly int _gridSize;

		public bool UpdateTimer()
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

		public string Serialize() {
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

		public static Grid Deserialize(string s)
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

		public Grid(int size)
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
		public int GetRow(int idx)
		{
			return idx / _columns;
		}

		// column # of item at idx
		public int GetColumn(int idx)
		{
			return idx % _columns;
		}

		public int GetIndex(int x, int y)
		{
			return y + x * _columns;
		}

		public Move MoveTile(int i, Direction d)
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
		

		// use memoization so we don't have to iterate every time we call GetDirections()
		private readonly List<HashSet<Direction>> Dir_Memos = new List<HashSet<Direction>>();

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
			} else
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

			if (moves.Count > 0)
			{
				int newTile = PlaceTile();
				moves.Add(new Spawn(newTile, Tiles[newTile]));
			}


			if ((Tiles.All(tile => tile.Value > 0) && moves.Count == 0) || Tiles.Any(tile => tile.Value == winningScore))
			{
				GameOver = true;
			}

			return moves;
		}

		public override string ToString()
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
