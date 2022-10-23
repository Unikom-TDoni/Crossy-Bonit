using System;
using UnityEngine;
using UnityEngine.Pool;

namespace Lncodes.Module.Unity.Pool
{
    [Serializable]
    public sealed class ObjectPoolFacade<T> where T : class
    {
        [SerializeField]
        private int _maxSize = default;

        [SerializeField]
        private bool _collectionCheck = default;

        [SerializeField]
        private int _defaultCapacity = default;

        [SerializeField]
        private ObjectPoolTypes _types = default;

        public IObjectPool<T> ObjectPool = default;

        public void Init(IObjectSpawner<T> objectSpawner)
        {
            switch (_types)
            {
                case ObjectPoolTypes.Stack:
                    ObjectPool = new ObjectPool<T>
                    (
                        objectSpawner.Create,
                        objectSpawner.OnTake,
                        objectSpawner.OnReturned,
                        objectSpawner.Destroy,
                        _collectionCheck,
                        _defaultCapacity,
                        _maxSize
                    );
                    break;
                case ObjectPoolTypes.LinkedList:
                    ObjectPool = new LinkedPool<T>
                    (
                        objectSpawner.Create,
                        objectSpawner.OnTake,
                        objectSpawner.OnReturned,
                        objectSpawner.Destroy,
                        _collectionCheck,
                        _maxSize
                    );
                    break;
                case ObjectPoolTypes.Default:
                    throw new ArgumentOutOfRangeException("Choose Pool Types First");
            }
        }
    }
}