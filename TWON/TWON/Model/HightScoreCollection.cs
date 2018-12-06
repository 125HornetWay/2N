using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace TWON.Model
{
    public class HightScoreCollection
    {
		public static List<HighScore> HighScores = new List<HighScore>();
		public HightScoreCollection()
		{

		}

		public void OrganiseList()
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

		public static void Save()
		{
			
			foreach (HighScore score in  HighScores)
			{
				string data = score.Name + ' ' + score.Score;
				using (StreamWriter writer = new StreamWriter("HighScores.txt"))
				{
					writer.WriteLine(data);
				}
				
			}
		}

		public static List<string> Load()
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
