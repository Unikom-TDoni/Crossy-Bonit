using UnityEngine;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Edu.CrossyBox.Environment
{
    public sealed class TreeHolder : MonoBehaviour
    {
        private readonly Collection<GameObject> _trees = new();

        public void Add(GameObject tree) =>
            _trees.Add(tree);

        public IReadOnlyList<GameObject> GetTrees() =>
            _trees;
    }
}