using System;
using Xamarin.Forms;
// Represents a Tile

namespace TWON
{
	public class Tile
	{
		public int Value { get; set; }

		public string Serialize()
		{
			return Convert.ToString(Value);
		}

		public static Tile Deserialize(string val)
		{
			var tile = new Tile();
			tile.Value = Convert.ToInt32(val);
			return tile;
		}
		
		public Color GetColor()
		{
			switch (Value)
			{
				case 4:
					return Color.FromHex("#ede0c8");
				case 8:
					return Color.FromHex("#f2b179");
				case 16:
					return Color.FromHex("#f59563");
				case 32:
					return Color.FromHex("#f67c5f");
				case 64:
					return Color.FromHex("#f65e3b");
				case 128:
					return Color.FromHex("#edcf72");
				case 256:
					return Color.FromHex("#edcc61");
				case 512:
					return Color.FromHex("#edc850");
				case 1024:
					return Color.FromHex("#edc53f");
				case 2048:
					return Color.FromHex("#edc22e");
				default:
					return Color.FromHex("#eee4da");
			}
		}
	}
}
