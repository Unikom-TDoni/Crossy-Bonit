using System;
using UnityEngine;

namespace Edu.CrossyBox.Interaction
{
    public sealed class InputHandler
    {
        private GameInputActions _gameInputActions = default;

        public void Activate(GameInputActions.IActorActions actorActions)
        {
            _gameInputActions = new();
            _gameInputActions.Enable();
            _gameInputActions.Actor.SetCallbacks(actorActions);
        }

        public void Deactive() { 
            _gameInputActions.Disable();
        }
    }

}