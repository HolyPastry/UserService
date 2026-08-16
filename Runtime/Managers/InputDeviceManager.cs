using System;
using KBCore.Refs;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;

namespace Bakery
{
    public class InputDeviceManager : ValidatedMonoBehaviour, IDeviceManager
    {
        private const string InputMapKey = "ControlInputMap";
        private const string GamepadTypeKey = "GamepadType";
        [SerializeField, Self] private PlayerInput _playerInput;

        public EnumInputScheme InputScheme => _currentInputScheme;
        public EnumGamepadType GamepadType => _currentGamepadType;

        private EnumInputScheme _currentInputScheme = EnumInputScheme.KeyboardAndMouse;
        private EnumGamepadType _currentGamepadType = EnumGamepadType.Xbox;
        void Awake()
        {
            User.Device = () => this;
        }
        void OnEnable()
        {
            LoadInputSchemePrefs();
            _playerInput.onControlsChanged += OnControlChanged;
        }

        void OnDisable()
        {
            _playerInput.onControlsChanged -= OnControlChanged;
        }

        void OnDestroy()
        {
            User.Device = User.UnregisterDeviceManager;
        }

        private void LoadInputSchemePrefs()
        {
            try
            {
                if (PlayerPrefs.HasKey(InputMapKey))
                {
                    _currentInputScheme = (EnumInputScheme)Enum.Parse(typeof(EnumInputScheme), PlayerPrefs.GetString(InputMapKey));
                }
                if (PlayerPrefs.HasKey(GamepadTypeKey))
                {
                    _currentGamepadType = (EnumGamepadType)Enum.Parse(typeof(EnumGamepadType), PlayerPrefs.GetString(GamepadTypeKey));
                }
            }
            catch (Exception)
            {
                Debug.LogWarning($"Failed to load input scheme: {_currentInputScheme}");
            }
        }

        private void OnControlChanged(PlayerInput input)
        {
            _currentInputScheme = input.currentControlScheme switch
            {
                "Keyboard&Mouse" => EnumInputScheme.KeyboardAndMouse,
                "Gamepad" => EnumInputScheme.Gamepad,
                "Touch" => EnumInputScheme.Touch,
                _ => EnumInputScheme.KeyboardAndMouse
            };
            if (_currentInputScheme == EnumInputScheme.Gamepad)
                SetGamepadType();

            PlayerPrefs.SetString(InputMapKey, _currentInputScheme.ToString());
            User.Events.Device.OnChanged();
        }

        private void SetGamepadType()
        {
            Gamepad _gamepad = _playerInput.GetDevice<Gamepad>();
            if (_gamepad == null) return;

            if (_gamepad is DualShockGamepad dualShock)
                _currentGamepadType = EnumGamepadType.PlayStation;
            else
                _currentGamepadType = EnumGamepadType.Xbox;

            PlayerPrefs.SetString(GamepadTypeKey, _currentGamepadType.ToString());
        }

    }
}
