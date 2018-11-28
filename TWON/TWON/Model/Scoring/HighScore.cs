using System.Collections.Generic;
using System.IO;
using TWON.Model;

namespace TWON
{
	public class HighScore
	{
	
		public string Name { get; set; }
		public int Score { get; set; }
		static int CurrentHighestScore;

		//Constructor for the Highscore class
		public HighScore(string name, int score)
		{
			this.Name = name;
			this.Score = score;
		}

		//Checks if player's score is a highscore.
		public bool CheckIfHighScore(string RecievedName, int RecievedScore)
		{
			bool High = false;
			int Counter = 0;
			HighScore NewHighScore;

			while (High == false || HightScoreCollection.HighScores.Count == 0)
			{	if(HightScoreCollection.HighScores.Count == 0)
				{
					NewHighScore = new HighScore(RecievedName, RecievedScore);
					HightScoreCollection.HighScores.Add(NewHighScore);
					High = true;
				}
				 
				else if (HightScoreCollection.HighScores[Counter].Score < RecievedScore)
				{
					NewHighScore = new HighScore(RecievedName, RecievedScore);
					HightScoreCollection.HighScores.Add(NewHighScore);
					High = true;
				}
				++Counter;
			}

			return High;
		}
		

		//Adds a player's name and score to  high score file
		

		//Returns all the high scores and correspoinding player names from a file
		
		//Returns the best score from a file to be displayed on the leader board. 
		public string SetBestScore(int NewScore)
		{
			if (NewScore > CurrentHighestScore)
			{
				CurrentHighestScore = NewScore;
			}
			return NewScore.ToString();
		}
	}
}
