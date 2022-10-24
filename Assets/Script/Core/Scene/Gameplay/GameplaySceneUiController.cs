using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

        [SerializeField]
        private Button _btnRestart = default;

        [SerializeField]
        private Button _btnExit = default;

        public void OnAwake()
        {
            _btnExit.onClick.AddListener(() => SceneManager.LoadScene(GameManager.Instance.SceneObjects.MainMenu));
            _btnRestart.onClick.AddListener(() => SceneManager.LoadScene(GameManager.Instance.SceneObjects.Gameplay));
        }

        public void UpdateScoreText(int score) =>
            _txtInGameScore.text = score.ToString();

        public void ShowGameOver(int score, int highScore)
        {
            _gameOver.SetActive(true);
            _txtGameOverScore.text = $"Score : {score}";
            _txtGameOverHighScore.text = $"High Score : {highScore}";
        }
    }
}
