using UnityEngine;

namespace Edu.CrossyBox.Environment
{
    public sealed class GroundController : MonoBehaviour
    {
        [field: SerializeField]
        public int Size { get; private set; } = default;

        [field:SerializeField]
        public GroundTypes Types { get; private set; } = default;
    }
}