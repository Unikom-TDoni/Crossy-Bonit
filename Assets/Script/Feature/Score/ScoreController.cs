using UnityEngine;

namespace Edu.CrossyBox.Score
{
    public sealed class ScoreController
    {
        private const string StorageKey = "Score";

        public int Score { get; private set; } = default;

        public void IncreaseScore() =>
            Score++;

        public void SaveHighScore()
        {
            if (Score > GetHighScore()) 
                PlayerPrefs.SetInt(StorageKey, Score);
        }

        public int GetHighScore() =>
            PlayerPrefs.GetInt(StorageKey, default);
    }
}