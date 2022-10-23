using System;
using UnityEngine;
using Lncodes.Module.Unity.Pool;
using Random = UnityEngine.Random;
using Object = UnityEngine.Object;

namespace Edu.CrossyBox.Environment
{
    [Serializable]
    public sealed class GroundSpawner : IObjectSpawner<GroundController>
    {
        [SerializeField]
        private GameObject[] _groundObj = default;

        [SerializeField]
        private TreeSpawner _treeSpawner = default;

        public GroundController Create()
        {
            var randomRoadObj = _groundObj[Random.Range(0, _groundObj.Length)];
            var obj = Object.Instantiate(randomRoadObj).GetComponent<GroundController>();
            if(obj.Types is GroundTypes.Grass)
                _treeSpawner.Reset(obj.GetComponentInChildren<TreeHolder>());
            return obj;
        }

        public void Destroy(GroundController pooledObj) =>
            Object.Destroy(pooledObj);

        public void OnReturned(GroundController pooledObj) =>
            pooledObj.gameObject.SetActive(false);

        public void OnTake(GroundController pooledObj)
        {
            if (pooledObj.Types is GroundTypes.Grass)
                _treeSpawner.Reset(pooledObj.GetComponentInChildren<TreeHolder>());
            pooledObj.gameObject.SetActive(true);
        }
    }
}