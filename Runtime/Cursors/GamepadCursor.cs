using System;
using UnityEngine;
using UnityEngine.UI;

namespace Bakery
{

    public class GamepadCursor : MonoBehaviour, ICursor
    {
        [SerializeField] private RawImage _cursorImage;
        [SerializeField] private float _movementSpeed = 1000f;
        private Vector2 _delta;
        private bool _isActive;
        private Canvas _canvas;

        private Vector2 ScaledScreenSize => new(Screen.width / _canvas.scaleFactor,
                                        Screen.height / _canvas.scaleFactor);

        private Vector2 ScaledPosition => _cursorImage.rectTransform.anchoredPosition * _canvas.scaleFactor;

        void Awake()
        {
            _canvas = GetComponentInParent<Canvas>();
        }
        void OnValidate()
        {
            if (_cursorImage == null)
                Debug.LogWarning("Cursor image reference is missing. Please assign a RawImage component to the GamepadCursor script.", this);
        }
        void Start()
        {
            Warp(new Vector2(ScaledScreenSize.x / 2f, ScaledScreenSize.y / 2f));
        }

        void FixedUpdate()
        {
            if (_delta != Vector2.zero)
            {

                var newPosition = Vector2.Lerp(_cursorImage.rectTransform.anchoredPosition,
                                                _cursorImage.rectTransform.anchoredPosition + _delta * _movementSpeed, Time.fixedDeltaTime);



                newPosition = new Vector2(Mathf.Clamp(newPosition.x, 0, ScaledScreenSize.x),
                                            Mathf.Clamp(newPosition.y, 0, ScaledScreenSize.y));
                _cursorImage.rectTransform.anchoredPosition = newPosition;
            }
            // if (_canvas.scaleFactor < 0)
            //     _cursorImage.rectTransform.localScale = Vector3.one / _canvas.scaleFactor;
        }



        public void SetVisibility(bool isVisible)
        {
            _cursorImage.enabled = _isActive && isVisible;
        }

        public void SetLockState(bool isLocked)
        {
            // Gamepad cursor doesn't need to lock the system cursor, so this can be left empty or used to manage internal state if necessary.
        }
        public void SetDeltaPosition(Vector2 delta)
        {
            _delta = delta;

        }
        public void Warp(Vector2 position)
        {
            _cursorImage.rectTransform.anchoredPosition = position / _canvas.scaleFactor;
        }

        public void SetActive(bool isActive)
        {
            _isActive = isActive;
            //_cursorImage.gameObject.SetActive(isActive);

        }

        internal Vector2 GetPosition()
        {
            return ScaledPosition;
        }

        public void SetCursor(CursorType cursorType)
        {
            _cursorImage.texture = cursorType.Texture;
            _cursorImage.uvRect = new Rect(cursorType.Hotspot, Vector2.one);
        }
    }
}