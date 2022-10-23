using UnityEngine;

namespace Edu.CrossyBox.Core
{
    public sealed class MainMenuSceneCoordinator : MonoBehaviour
    {
        [SerializeField]
        private MainMenuSceneUiController _uiController = default;

        private void Awake()
        {
            _uiController.OnAwake();
        }
    }
}
