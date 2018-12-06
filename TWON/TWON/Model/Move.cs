using System;
using System.Collections.Generic;
using System.Text;

// File: Move.cs
// Description: abstract class for representing a move

namespace TWON.Model
{
	public abstract class Move
	{
		// Original index of piece to be moved
		public int i;

		// Tile moved
		public Tile tile;
	}
}
