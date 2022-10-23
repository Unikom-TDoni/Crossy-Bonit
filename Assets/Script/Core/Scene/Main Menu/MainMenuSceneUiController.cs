using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Edu.CrossyBox.Core
{
    [Serializable]
    public sealed class MainMenuSceneUiController
    {
        [SerializeField]
        private Button _btnStart = default;

        [SerializeField]
        private Button _btnSetting = default;

        [SerializeField]
        private Button _btnExit = default;

        public void OnAwake()
        {
            _btnExit.onClick.AddListener(() => Application.Quit());
            _btnStart.onClick.AddListener(() => SceneManager.LoadSceneAsync(GameManager.Instance.SceneObjects.Gameplay));
        }
    }
}