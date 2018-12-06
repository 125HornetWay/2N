using System;
using System.Collections.Generic;
using System.Text;

namespace TWON.Model
{
	// Represent a new tile
    class Spawn : Move
    {
		public Spawn (int i, Tile tile)
		{
			this.i = i;
			this.tile = tile;
		}
    }
}
