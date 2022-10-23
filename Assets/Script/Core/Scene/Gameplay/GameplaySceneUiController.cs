using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Edu.CrossyBox.Core
{
    [Serializable]
    public sealed class GameplaySceneUiController
    {
        [SerializeField]
        private TextMeshProUGUI _txtInGameScore = default;

        [SerializeField]
        private GameObject _gameOver = default;

        [SerializeField]
        private TextMeshProUGUI _txtGameOverScore = default;

        [SerializeField]
        private TextMeshProUGUI _txtGameOverHighScore = default;

        public void UpdateScoreText(int score) =>
            _txtInGameScore.text = score.ToString();

        public void ShowGameOver(int score, int highScore)
        {
            _gameOver.SetActive(true);
            _txtGameOverScore.text = score.ToString();
            _txtGameOverHighScore.text = highScore.ToString();
        }
    }
}
