using System.Collections.Generic;
using UnityEngine;
using Bakery.Core;
using UnityEngine.InputSystem;
using System.Collections;

namespace Bakery
{

    public class CursorManager : MonoBehaviour, ICursorManager
    {
        [SerializeField] private InputActionReference _mouseMoveAction;
        [SerializeField] private InputActionReference _gamepadCursorMoveAction;
        [SerializeField] private bool _onByDefault = false;
        [SerializeField] private CursorType _defaultCursor;
        [SerializeField] private List<CursorType> _cursorTypes;
        [SerializeField] private float _sleepTimer = 5f;
        [SerializeField] private bool _forceCursorOn;
        [SerializeField] private bool _activateSleepMode = true;
        [SerializeField] private GamepadCursor _gamepadCursor;

        private bool _overriding;
        private readonly List<ICursorAttachable> _attached = new();
        private readonly List<ICursor> _cursors = new();

        private Vector2 _storedPosition;
        private int _visibilityPoint;
        private int _unlockPoint;

        private bool _sleepMode;
        private Vector2 _prevPosition;
        private float _sleepTime;

        private bool VisibilityStored => _visibilityPoint > 0;
        private bool UnlockedStored => _unlockPoint > 0;

        public Vector2 Position
        {
            get => GetPosition();
            set => Warp(value);
        }

        void Awake()
        {
            var mouseCursor = new MouseCursor();
            _cursors.Add(mouseCursor);
            if (_gamepadCursor != null)
                _cursors.Add(_gamepadCursor);

            _sleepTime = Time.time;
        }
        void OnEnable()
        {
            User.Cursor = () => this;
            User.Events.Device.OnChanged += OnDeviceChanged;
        }

        void OnDisable()
        {
            User.Cursor = User.UnregisterCursorManager;
            User.Events.Device.OnChanged -= OnDeviceChanged;
        }
        IEnumerator Start()
        {
            yield return null;
            if (_onByDefault)
            {
                SetVisibility(true);
                Unlock(true);
            }

            OnDeviceChanged();
            _prevPosition = GetPosition();
            _sleepTime = Time.time;
        }

        private Vector2 GetPosition()
        {
            if (User.Device().InputScheme is EnumInputScheme.Gamepad)
            {
                var deltaPosition = _gamepadCursorMoveAction.action.ReadValue<Vector2>();
                _gamepadCursor.SetDeltaPosition(deltaPosition);
                return _gamepadCursor.GetPosition();
            }
            else
            {
                var position = _mouseMoveAction.action.ReadValue<Vector2>();
                _gamepadCursor.Warp(position);
                return position;
            }
        }

        private void Warp(Vector2 vector)
        {
            _cursors.ForEach(x => x.Warp(vector));
        }

        public void Attach(ICursorAttachable attachable)
            => _attached.AddUnique(attachable);

        public void Detach(ICursorAttachable attachable)
            => _attached.Remove(attachable);

        public void StorePosition()
            => _storedPosition = GetPosition();

        public void RecallPosition()
            => Warp(_storedPosition);

        public void ResetPosition()
            => Warp(new Vector2(Screen.width / 2, Screen.height / 2));

        public void WakeUp()
        {
            _sleepMode = false;
            _sleepTime = Time.time;
            User.Events.Cursor.OnWake.Invoke();
        }
        public void PutToSleep()
        {
            _sleepMode = true;
            _sleepTime = Time.time;
            User.Events.Cursor.OnSleep.Invoke();
        }
        private void OnDeviceChanged()
        {
            if (User.Device().InputScheme is EnumInputScheme.Gamepad)
                _cursors.ForEach(x => x.SetActive(x as GamepadCursor != null));
            else
                _cursors.ForEach(x => x.SetActive(x as MouseCursor != null));
        }


        private void SleepModeUpdate()
        {
            if (_sleepMode && _prevPosition != GetPosition())
                WakeUp();

            if (!_sleepMode &&
                    _prevPosition == GetPosition() &&
                    Time.time - _sleepTime > _sleepTimer)
                PutToSleep();
            _prevPosition = GetPosition();
        }

        void FixedUpdate()
        {
            if (_activateSleepMode)
                SleepModeUpdate();
            StateUpdate();
            AttachObjectUpdate();
            IconUpdate();
        }

        public void Unlock(bool isLocked)
        {
            if (isLocked)
                _unlockPoint++;
            else
                _unlockPoint--;
            _unlockPoint = Mathf.Max(0, _unlockPoint);
        }

        public void SetVisibility(bool isOn)
        {
            if (isOn)
                _visibilityPoint++;
            else
                _visibilityPoint--;
            _visibilityPoint = Mathf.Max(0, _visibilityPoint);
        }
        private void StateUpdate()
        {
            if (_sleepMode)
            {
                _cursors.ForEach(x => x.SetVisibility(false));
                _cursors.ForEach(x => x.SetLockState(false));
                return;
            }
            if (_attached.Count <= 0 || _forceCursorOn)
            {
                _cursors.ForEach(x => x.SetVisibility(VisibilityStored));
                _cursors.ForEach(x => x.SetLockState(!UnlockedStored));
            }
            else
            {
                _cursors.ForEach(x => x.SetVisibility(false));
                _cursors.ForEach(x => x.SetLockState(false));
            }
        }

        private void AttachObjectUpdate()
        {
            int index = 0;
            var position = Position;
            while (index < _attached.Count)
            {
                if (_attached[index] == null)
                {
                    _attached.RemoveAt(index);
                    continue;
                }
                _attached[index].UpdatePosition(position);
                index++;
            }
        }

        private void IconUpdate()
        {
            if (_overriding) return;
            var obj = User.Raycast().HoveredObject;
            if (obj == null ||
                !obj.TryGetComponent<CustomCursor>(out var customCursor) ||
                customCursor.enabled == false
                )
                SetCursor(_defaultCursor);
            else SetCursor(customCursor.Cursor);
        }

        public void Override(CursorType type)
        {
            _overriding = true;
            SetCursor(type);
        }

        public void RemoveOverride()
        {
            _overriding = false;
            SetCursor(_defaultCursor);
        }

        private void SetCursor(CursorType type)
        {
            var cursor = _cursorTypes.Find(x => x == type);
            if (cursor.Texture == null)
            {
                Debug.LogError($"Cursor of type {type} not found");
                return;
            }
            Texture2D texture = cursor.Texture;

            _cursors.ForEach(x => x.SetCursor(cursor));
        }
    }
}