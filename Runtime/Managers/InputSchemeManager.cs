using KBCore.Refs;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Bakery.Inputs
{

    [RequireComponent(typeof(PlayerInput))]
    public class InputSchemeManager : ValidatedMonoBehaviour, IInputSchemeManager
    {
        [SerializeField, Self] private PlayerInput _playerInput;

        public bool IsPlayerMovementDisabled => !_playerInput.inputIsActive;

        void Awake()
        {
            User.Scheme = () => this;
        }
        void OnDestroy()
        {
            User.Scheme = User.UnregisterInputSchemeManager;
        }

        public void ToggleControls(bool isOn)
        {
            if (isOn)
                _playerInput.ActivateInput();
            else
                _playerInput.DeactivateInput();
        }

    }
}
