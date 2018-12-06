using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

// File: HighScoreCollection.cs
// Description: Manipulates High Score list
namespace TWON.Model
{
	// Program's interface with the High Score collection
    public class HightScoreCollection
    {
		public static List<HighScore> HighScores = new List<HighScore>(); // list of high scores
		public HightScoreCollection()
		{

		}

		public void OrganiseList() // Organize the list for display
		{
			int MaximumSize = 10;
			if (HighScores.Count > MaximumSize)
			{
				int LeastScore = 0;
				HighScore Smallest = null;
				foreach (HighScore item in HighScores)
				{
					if (item.Score < LeastScore)
					{
						LeastScore = item.Score;
						Smallest = item;
					}
				}
				HighScores.Remove(Smallest);
			}
			//sort list by scores
		}

		public void Save(List<HighScore> HighScores) // Save high scores to file
		{
			int count = 1;
			foreach (HighScore score in HighScores)
			{
				string data = count.ToString() + ' ' + score.Name + ' ' + score.Score;
				using (StreamWriter writer = new StreamWriter("HighScores.txt"))
				{
					writer.WriteLine(data);
				}
				++count;
			}
		}

		public List<string> Load() // load scores from file
		{
			List<string> HighScoreData = new List<string>();
			string line = "nothing yet";
			while (line != "" && line != null)
			{
				using (StreamReader rd = new StreamReader("HighScores.txt"))
				{
					line = rd.ReadLine();
					HighScoreData.Add(line);
				}
			}

			return HighScoreData;
		}


	}
}
