using System;
using UnityEngine;
using Lncodes.Module.Unity.Pool;
using Object = UnityEngine.Object;

namespace Edu.CrossyBox.Environment
{
    [Serializable]
    public sealed class CarObjectSpawner : IObjectSpawner<CarController>
    {
        [SerializeField]
        private GameObject _carObj = default;

        public CarController Create()
        {
            var obj = Object.Instantiate(_carObj);
            return obj.GetComponent<CarController>();
        }

        public void Destroy(CarController pooledObj) =>
            Object.Destroy(pooledObj);

        public void OnReturned(CarController pooledObj) =>
            pooledObj.gameObject.SetActive(false);

        public void OnTake(CarController pooledObj) =>
            pooledObj.gameObject.SetActive(true);
    }
}