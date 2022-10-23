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
        public event Action<IEnumerable<Vector3>> OnSpawnRoad = default;

        [SerializeField]
        private GroundSpawner _groundSpawner = default;

        [SerializeField]
        public Boundary<float> _carSpawnPoint = default;

        [SerializeField]
        private Boundary<float> _spawnBoundaryZPosition = default;

        [SerializeField]
        private ObjectPoolFacade<GroundController> _groundObjPool = default;

        private readonly LinkedList<GroundController> _inGameGround = new();

        public void OnAwake()
        {
            _groundObjPool.Init(_groundSpawner);
            CreateFirstGround();
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

        private void ActiveGround()
        {
            var lastActiveObj = _inGameGround.Last.Value;
            if (lastActiveObj.transform.position.z > _spawnBoundaryZPosition.Max) return;
            var obj = _groundObjPool.ObjectPool.Get();
            if (lastActiveObj.Types is GroundTypes.Road)
            {
                _groundObjPool.ObjectPool.Release(obj);
                obj = GetUniqueGroundTypeExcept(GroundTypes.Road);
            }
            var maxSize = Math.Max(lastActiveObj.Size, obj.Size);
            obj.transform.position = new Vector3(obj.transform.position.x, default, lastActiveObj.transform.position.z + maxSize);
            if (obj.Types is GroundTypes.Road) {
                if (obj.Size == 2)
                    OnSpawnRoad(new Vector3[] { 
                        new Vector3(GetRandomXCarSpawnPoint(), default, obj.transform.position.z) 
                    });
                else
                    OnSpawnRoad(new Vector3[] { 
                        new Vector3(GetRandomXCarSpawnPoint(), default, obj.transform.position.z - 1),
                        new Vector3(GetRandomXCarSpawnPoint(), default, obj.transform.position.z + 1),
                    });
            }
            _inGameGround.AddLast(obj);
        }

        private void DeactiveGrounds()
        {
            foreach (var item in _inGameGround)
            {
                if(item.transform.position.z < _spawnBoundaryZPosition.Min)
                {
                    _groundObjPool.ObjectPool.Release(item);
                    _inGameGround.Remove(item);
                }
            }
        }

        private float GetRandomXCarSpawnPoint() =>
             Random.value < 0.5f ? _carSpawnPoint.Min : _carSpawnPoint.Max;

        private void CreateFirstGround()
        {
            var obj = _groundObjPool.ObjectPool.Get();
            obj.transform.position = new Vector3(default, default, _spawnBoundaryZPosition.Min);
            _inGameGround.AddLast(obj);
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