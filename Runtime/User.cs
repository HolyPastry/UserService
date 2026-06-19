using System;
using System.Collections.Generic;

using UnityEngine;

namespace Bakery
{
    public static class User
    {
        public static class Events
        {
            public static class Device
            {
                public static Action OnChanged = delegate { };
            }
            public static class Cursor
            {
                public static Action<GameObject> OnEnter = delegate { };
                public static Action<GameObject> OnExit = delegate { };
                public static Action OnSleep = delegate { };
                public static Action OnWake = delegate { };
                public static Action OnMove = delegate { };
            }
        }
        public static Func<ICursorManager> Cursor = UnregisterCursorManager;
        public static Func<ICursorRaycast> Raycast = UnregisterCursorRaycast;
        public static Func<IDeviceManager> Device = UnregisterDeviceManager;
        public static Func<IInputSchemeManager> Scheme = UnregisterInputSchemeManager;


        private static ICursorManager _cursorManager;
        private static ICursorRaycast _cursorRaycast;
        private static IDeviceManager _deviceManager;
        private static IInputSchemeManager _inputSchemeManager;

        public static IInputSchemeManager UnregisterInputSchemeManager()
        {

            Debug.Log("InputSchemeManager not found. Returning dummy implementation.");
            _inputSchemeManager ??= new InputSchemeManagerDummy();
            Scheme = () => _inputSchemeManager;
            return _inputSchemeManager;
        }

        public static IDeviceManager UnregisterDeviceManager()
        {

            Debug.Log("DeviceManager not found. Returning dummy implementation.");
            _deviceManager ??= new DeviceManagerDummy();
            Device = () => _deviceManager;
            return _deviceManager;
        }
        public static ICursorRaycast UnregisterCursorRaycast()
        {

            Debug.Log("CursorRaycast not found. Returning dummy implementation.");

            _cursorRaycast ??= new CursorRaycastDummy();
            Raycast = () => _cursorRaycast;
            return _cursorRaycast;
        }
        public static ICursorManager UnregisterCursorManager()
        {
            _cursorManager ??= new CursorManagerDummy();
            Debug.Log("CursorManager not found. Returning dummy implementation.");
            Cursor = () => _cursorManager;

            return _cursorManager;
        }

        private class InputSchemeManagerDummy : IInputSchemeManager
        {
            public bool IsPlayerMovementDisabled => false;
            public void ToggleControls(bool isOn) { }
        }

        private class DeviceManagerDummy : IDeviceManager
        {
            public EnumInputScheme InputScheme => EnumInputScheme.KeyboardAndMouse;
            public EnumGamepadType GamepadType => EnumGamepadType.Unknown;
        }

        private class CursorRaycastDummy : ICursorRaycast
        {
            public GameObject HoveredObject => null;
            public IEnumerable<GameObject> HoveredObjects(Predicate<GameObject> predicate)
                => Array.Empty<GameObject>();
            public bool IsHovering(GameObject obj) => false;
            public bool IsOverUI(GameObject obj) => false;
            public bool IsOverUI() => false;

            public LayerMask InteractableLayer => 0;
        }
        private class CursorManagerDummy : ICursorManager
        {
            public Vector2 Position { get => Vector2.zero; set { } }
            public void Attach(ICursorAttachable transform) { }
            public void Detach(ICursorAttachable transform) { }
            public void Override(CursorType type) { }
            public void PutToSleep() { }
            public void RecallPosition() { }
            public void RemoveOverride() { }
            public void ResetPosition() { }
            public void SetVisibility(bool isOn) { }
            public void StorePosition() { }
            public void Unlock(bool unlock) { }
            public void WakeUp() { }
        }

        //Cleaning stuff in case cowboys are fast reloading in the editor
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void ResetStatics()
        {
            Events.Cursor.OnEnter = delegate { };
            Events.Cursor.OnExit = delegate { };
            Events.Cursor.OnSleep = delegate { };
            Events.Cursor.OnWake = delegate { };
            Events.Device.OnChanged = delegate { };

            Cursor = UnregisterCursorManager;
            Raycast = UnregisterCursorRaycast;
            Device = UnregisterDeviceManager;
            Scheme = UnregisterInputSchemeManager;

#if UNITY_EDITOR
            Debug.Log("[User] Static fields reset (domain reload skipped)");
#endif
        }
    }
}