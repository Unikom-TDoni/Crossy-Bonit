using System;
using UnityEngine;

namespace Edu.CrossyBox.Environment
{
    public sealed class CarController : MonoBehaviour
    {
        [NonSerialized]
        public float Speed = default;

        private void Update()
        {
            transform.Translate(Vector3.right * (Speed * Time.deltaTime));
        }
    }
}
