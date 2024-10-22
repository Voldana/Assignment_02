using UnityEngine;

namespace Project.Scripts
{
    public class ScoreManager
    {
        private const string ScoreKey = "lvl";


        public void SetNewHighScore(string level, int score)
        {
            PlayerPrefs.SetInt(ScoreKey + level, score);
        }

        public int GetHighScore(string level)
        {
            return PlayerPrefs.GetInt(ScoreKey + level);
        }
    }
}