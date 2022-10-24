using UnityEngine;
using System.Collections;
using Edu.CrossyBox.Score;
using Edu.CrossyBox.Player;
using Edu.CrossyBox.Environment;
using Edu.CrossyBox.Interaction;
using System.Collections.Generic;

namespace Edu.CrossyBox.Core
{
    public sealed class GameplaySceneCoordinator : MonoBehaviour
    {
        [Header("Reguler Script")]
        [SerializeField]
        private CarHandler _carHandler = default;

        [SerializeField]
        private GroundHandler _groundHandler = default;

        [SerializeField]
        private GameplaySceneUiController _gameplayUiController = default;

        [Header("Mono Script")]
        [SerializeField]
        private PlayerController _playerController = default;

        [SerializeField]
        private GameplayAudioController _gameplayAudioController = default;

        private readonly InputHandler _inputHandler = new();

        private readonly ScoreController _scoreController = new();

        private Dictionary<GroundController, IEnumerator> _activeCarSpawnerCoroutine = new();

        private void Awake()
        {
            _carHandler.OnAwake();
            _gameplayUiController.OnAwake();
            StartCoroutine(_gameplayAudioController.PlaySfxInRandom());
            _groundHandler.OnAwake(position =>
            {
                foreach (var item in position)
                    StartCoroutine(_carHandler.AutoSpawnCar(item));
            });

            _playerController.gameObject.SetActive(true);
            _playerController.SetPlayerCallback(playerZPosition =>
            {
                if(_scoreController.Score < playerZPosition)
                {
                    _scoreController.IncreaseScore();
                    _groundHandler.IncreaseBoundary();
                    _gameplayUiController.UpdateScoreText(_scoreController.Score);
                }
            }, () =>
            {
                _inputHandler.Deactive();
                _scoreController.SaveHighScore();
                _gameplayUiController.ShowGameOver(_scoreController.Score, _scoreController.GetHighScore());
            });
        }

        private void OnEnable()
        {
            _inputHandler.Activate(_playerController);
        }

        private void Update()
        {
            _carHandler.OnUpdate();
            _groundHandler.OnUpdate();
        }

        private void OnDisable()
        {
            _inputHandler.Deactive();
            StopAllCoroutines();
        }
    }
}

