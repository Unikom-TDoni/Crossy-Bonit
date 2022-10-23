using System;
using UnityEngine;
using System.Collections;
using Lncodes.Module.Unity.Pool;
using Lncodes.Module.Unity.Helper;
using Random = UnityEngine.Random;
using System.Collections.Generic;
using static UnityEditor.Progress;
using System.Linq;

namespace Edu.CrossyBox.Environment
{
    [Serializable]
    public sealed class CarHandler
    {
        [SerializeField]
        private Boundary<float> _moveBoundary = default;

        [SerializeField]
        private Boundary<float> _speedBoundary = default;

        [SerializeField]
        private Boundary<float> _restSpawnTimeBoundary = default;

        [SerializeField]
        private CarObjectSpawner _carObjectSpawner = default;

        [SerializeField]
        private ObjectPoolFacade<CarController> _carObjectPool = default;

        private readonly List<CarController> _inGameCarObj = new();

        public void OnAwake()
        {
            _carObjectPool.Init(_carObjectSpawner);
        }

        public void OnUpdate()
        {
            for (int i = 0; i < _inGameCarObj.Count; i++)
            {
                var item = _inGameCarObj[i];
                if (item.transform.position.x < _moveBoundary.Min || item.transform.position.x > _moveBoundary.Max)
                {
                    _carObjectPool.ObjectPool.Release(item);
                    _inGameCarObj.Remove(item);
                }
            }
        }

        public IEnumerator AutoSpawnCar(Vector3 position)
        {
            var speed = Random.Range(_speedBoundary.Min, _speedBoundary.Max);
            var restTime = Random.Range(_restSpawnTimeBoundary.Min, _restSpawnTimeBoundary.Max);
            var rotation = position.x > 0 ? Quaternion.Euler(0, 180, 0) : Quaternion.Euler(0, 0, 0);
            while (true)
            {
                var obj = _carObjectPool.ObjectPool.Get();
                obj.Speed = speed;
                obj.transform.SetPositionAndRotation(position, rotation);
                _inGameCarObj.Add(obj);
                yield return new WaitForSeconds(restTime);
            }
        }
    }
}
