using System;
using UnityEngine;
using System.Collections.Generic;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Lncodes.Module.Unity.Helper;
using System.Collections.ObjectModel;

namespace Edu.CrossyBox.Environment
{
    [Serializable]
    public sealed class TreeSpawner
    {
        [SerializeField]
        private Mesh[] _meshes = default;

        [SerializeField]
        private GameObject _model = default;

        [SerializeField]
        private Boundary<int> _countBoundary = default;

        [SerializeField]
        private Boundary<int> _spawnXPositionBoundary = default;

        private Func<float, bool> _isValidToSpawnTree = default;

        public void OnAwake(Func<float, bool> isValidToSpawnTree) =>
            _isValidToSpawnTree = isValidToSpawnTree;

        public void Reset(TreeHolder holder)
        {
            var objects = holder.GetTrees();
            var newObjCount = GetRandomCount();
            var currentObjCount = objects.Count;

            var offset = newObjCount - currentObjCount;
            var uniqueXPosition = new Collection<float>();

            if (currentObjCount < newObjCount)
            {
                foreach (var item in objects)
                    ActiveObject(item, uniqueXPosition);

                for (int i = 0; i < offset; i++)
                    CreateObject(holder.transform, uniqueXPosition);
            }
            else
            {
                for (int i = 0; i < newObjCount; i++)
                {
                    if (i < offset)
                        objects[i].SetActive(false);
                    else
                        ActiveObject(objects[i], uniqueXPosition);
                }
            }
        }

        private void ChangeMesh(GameObject _model) =>
            _model.GetComponent<MeshFilter>().mesh = _meshes[Random.Range(0, _meshes.Length)];

        private void CreateObject(Transform holder, ICollection<float> uniquePosition)
        {
            var xPosition = GetRandomXPosition(uniquePosition);
            var obj = Object.Instantiate(_model,
                new Vector3(xPosition, default, default),
                Quaternion.identity);
            obj.transform.SetParent(holder);
            ChangeMesh(obj);
        }
            
        private void ActiveObject(GameObject obj, ICollection<float> uniquePosition)
        {
            ChangeMesh(obj);
            var xPosition = GetRandomXPosition(uniquePosition);
            obj.transform.position = new Vector3(xPosition, default, default);
            if (!obj.activeInHierarchy) return;
            obj.SetActive(true);
        }

        private int GetRandomCount() =>
            Random.Range(_countBoundary.Min, _countBoundary.Max);

        private int GetRandomXPosition(ICollection<float> uniquePosition)
        {
            int spawnXPosition;
            do {
                spawnXPosition = Random.Range(_spawnXPositionBoundary.Min, _spawnXPositionBoundary.Max);
                if (spawnXPosition % 2 != 0)
                    spawnXPosition += spawnXPosition < 0 ? 1 : -1;
            }
            while (uniquePosition.Contains(spawnXPosition));
            uniquePosition.Add(spawnXPosition);
            return spawnXPosition;
        }
    }
}