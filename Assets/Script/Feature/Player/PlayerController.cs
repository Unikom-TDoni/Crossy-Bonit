using Edu.CrossyBox.Core;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Edu.CrossyBox.Player
{
    public sealed class PlayerController : MonoBehaviour, GameInputActions.IActorActions
    {
        private Action<float> PlayerEndMoveCallback = default;

        [SerializeField]
        private float _speed = default;

        [SerializeField]
        private int _moveDestinationMultiplier = default;

        private bool _isDead = default;

        private Vector3 _destination = default;

        private Animator _animator = default;

        private CharacterController _characterController = default;

        private const string _jumpAnimationName = "Jump";

        private readonly int _jumpTriggerParameter = Animator.StringToHash("Jump");

        private void Awake()
        {
            _destination = transform.position;
            _animator = GetComponent<Animator>();
            _characterController = GetComponent<CharacterController>();
        }

        private void Update()
        {
            var offset = _destination - transform.position;
            if (offset.magnitude < 0.2f)
            {
                if (_characterController.enabled)
                {
                    _characterController.enabled = false;
                    transform.position = new Vector3(_destination.x, default, _destination.z);
                    PlayerEndMoveCallback?.Invoke(_destination.z / _moveDestinationMultiplier);
                }
            }
            else
            {
                _characterController.enabled = true;
                _characterController.Move(_speed * Time.deltaTime * offset.normalized);
            }
        }

        public void SetPlayerCallback(Action<float> playerEndMoveCallback) =>
            PlayerEndMoveCallback = playerEndMoveCallback;

        public void OnMovement(InputAction.CallbackContext context)
        {
            if (_isDead) return;
            if (!context.performed) return;
            if (_animator.GetCurrentAnimatorStateInfo(0).IsName(_jumpAnimationName)) return;
            _animator.SetTrigger(_jumpTriggerParameter);
            var inputPosition = context.ReadValue<Vector2>();
            var newXPosition = transform.position.x + inputPosition.x * _moveDestinationMultiplier;
            var newZPosition = transform.position.z + inputPosition.y * _moveDestinationMultiplier;
            _destination = new Vector3(newXPosition, default, newZPosition);
            transform.LookAt(new Vector3(_destination.x, transform.position.y, _destination.z));
        }

        private void OnTriggerEnter(Collider collider)
        {
            if (collider.CompareTag(GameManager.Instance.Tags.Car))
                _isDead = true;
        }
    }
}