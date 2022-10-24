using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Edu.CrossyBox.Core
{
    [Serializable]
    public sealed class GameplayAudioController
    {
        [SerializeField]
        private AudioSource _sfxAudioSource;

        public IEnumerator PlaySfxInRandom()
        {
            while (true)
            {
                yield return new WaitForSeconds(Random.Range(5, 10));
                _sfxAudioSource.Play();
            }
        }
    }
}
