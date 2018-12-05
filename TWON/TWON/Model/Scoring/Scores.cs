using System;
using System.Collections.Generic;
using System.Text;

namespace TWON.Model
{

	class Scores
	{
		public static int Score = 0;

		public static int SavedScore = 0;
		public Scores()
		{

		}

		//Takes the of tiles hat have been summed and adds them to the current score. 
		public static void UpdateScore(int value)
		{
			Score += value;

		}

		//Return the current score.
		public static int GetScore()
		{
			return Score;
		}

	}
}
