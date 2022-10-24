using System;
using UnityEngine;
using Lncodes.Module.Unity.Pool;
using System.Collections.Generic;
using Lncodes.Module.Unity.Helper;
using System.Collections.ObjectModel;
using Random = UnityEngine.Random;

namespace Edu.CrossyBox.Environment
{
    [Serializable]
    public sealed class GroundHandler
    {
        private Action<IEnumerable<Vector3>> _onSpawnRoad = default;

        [SerializeField]
        private GroundSpawner _groundSpawner = default;

        [SerializeField]
        public Boundary<float> _carSpawnPoint = default;

        [SerializeField]
        private Boundary<float> _spawnBoundaryZPosition = default;

        [SerializeField]
        private ObjectPoolFacade<GroundController> _groundObjPool = default;

        private readonly LinkedList<GroundController> _inGameGround = new();

        public void OnAwake(Action<IEnumerable<Vector3>> onSpawnRoad)
        {
            _groundObjPool.Init(_groundSpawner);
            _onSpawnRoad = onSpawnRoad;
            InitGround();
        }

        public void OnUpdate()
        {
            ActiveGround();
            //DeactiveGround();
        }

        public void IncreaseBoundary()
        {
            var firstGroundSize = _inGameGround.First.Value.Size;
            _spawnBoundaryZPosition.Max += firstGroundSize;
            _spawnBoundaryZPosition.Min += firstGroundSize;
        }

        private void InitGround()
        {
            var lastObj = GetUniqueGroundTypeExcept(GroundTypes.Road);
            lastObj.transform.position = new Vector3(default, default, _spawnBoundaryZPosition.Min);
            _inGameGround.AddLast(lastObj);
            while (lastObj.transform.position.z < 0)
            {
                var obj = GetUniqueGroundTypeExcept(GroundTypes.Road);
                obj.transform.position = new Vector3(default, default, lastObj.transform.position.z + Math.Max(lastObj.Size, obj.Size));
                _inGameGround.AddLast(obj);
                lastObj = obj;
            }
        }

        private void ActiveGround()
        {
            var lastActiveObj = _inGameGround.Last.Value;
            if (lastActiveObj.transform.position.z > _spawnBoundaryZPosition.Max) return;
            var obj = _groundObjPool.ObjectPool.Get();
            if (lastActiveObj.Types is GroundTypes.Road) obj = GetUniqueGroundTypeExcept(GroundTypes.Road);
            obj.transform.position = new Vector3(default, default, lastActiveObj.transform.position.z + Math.Max(lastActiveObj.Size, obj.Size));
            _inGameGround.AddLast(obj);
            if (obj.Types is GroundTypes.Road) SpawnCar(obj);
        }

        private void DeactiveGrounds()
        {
            foreach (var item in _inGameGround)
            {
                if (item.transform.position.z < _spawnBoundaryZPosition.Min)
                {
                    _groundObjPool.ObjectPool.Release(item);
                    _inGameGround.Remove(item);
                }
            }
        }

        private float GetRandomXCarSpawnPoint() =>
             Random.value < .5f ? _carSpawnPoint.Min : _carSpawnPoint.Max;

        private void SpawnCar(GroundController obj)
        {
            if (obj.Size == 2)
                _onSpawnRoad(new Vector3[] {
                        new Vector3(GetRandomXCarSpawnPoint(), default, obj.transform.position.z)
                    });
            else
                _onSpawnRoad(new Vector3[] {
                        new Vector3(GetRandomXCarSpawnPoint(), default, obj.transform.position.z - 1),
                        new Vector3(GetRandomXCarSpawnPoint(), default, obj.transform.position.z + 1),
                    });
        }

        private GroundController GetUniqueGroundTypeExcept(GroundTypes types)
        {
            var obj = _groundObjPool.ObjectPool.Get();
            var sameGroundTypes = new Collection<GroundController>();
            while (obj.Types == types)
            {
                sameGroundTypes.Add(obj);
                obj = _groundObjPool.ObjectPool.Get();
            }
            foreach (var item in sameGroundTypes)
                _groundObjPool.ObjectPool.Release(item);
            return obj;
        }
    }
}