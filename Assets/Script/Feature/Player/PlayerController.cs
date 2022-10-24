using Edu.CrossyBox.Core;
using Edu.CrossyBox.Environment;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using static UnityEditor.FilePathAttribute;

namespace Edu.CrossyBox.Player
{
    public sealed class PlayerController : MonoBehaviour, GameInputActions.IActorActions
    {
        private Action DeadCallback = default;

        private Action<float> EndMoveCallback = default;

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
            if (_isDead) return;
            var offset = _destination - transform.position;
            if (offset.magnitude > .2f)
            {
                offset = offset.normalized * _speed;
                _characterController.Move(offset * Time.deltaTime);
            }
            else
            {
                _characterController.enabled = false;
                transform.position = _destination;
                EndMoveCallback?.Invoke(_destination.z / _moveDestinationMultiplier);
                _characterController.enabled = true;
            }
        }

        public void SetPlayerCallback(Action<float> playerEndMoveCallback, Action deadCallback)
        {
            DeadCallback = deadCallback;
            EndMoveCallback = playerEndMoveCallback;
        }

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
            {
                transform.localScale = new Vector3(8, 1, 8);
                DeadCallback();
                _isDead = true;
                _animator.enabled = false;
            }
        }
    }
}