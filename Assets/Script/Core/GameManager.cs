using UnityEngine;
using UnityEngine.SceneManagement;
using Lncodes.Module.Unity.Template;

namespace Edu.CrossyBox.Core
{
    public sealed class GameManager : SingletonMonoBehavior<GameManager>
    {
        public Tags Tags = default;

        public SceneObjects SceneObjects = default;

        protected override void Awake()
        {
            base.Awake();
            SceneManager.LoadScene(SceneObjects.MainMenu);
        }
    }
}
