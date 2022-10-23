using System;
using Lncodes.Module.Unity.Editor;
using UnityEngine;

namespace Edu.CrossyBox.Core
{
    [Serializable]
    public struct Tags
    {
        [TagSelector]
        [SerializeField]
        private string _car;

        public string Car { get => _car; }
    }
}
