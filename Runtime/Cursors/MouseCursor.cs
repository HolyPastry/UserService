using UnityEngine;
using UnityEngine.InputSystem;

namespace Bakery
{
    public class MouseCursor : ICursor
    {
        private bool _isActive;

        public void SetCursor(CursorType cursorType)
        {
            Cursor.SetCursor(cursorType.Texture, cursorType.Hotspot, CursorMode.Auto);
        }

        public void SetVisibility(bool isVisible)
        {
            Cursor.visible = _isActive && isVisible;
        }

        public void SetLockState(bool isLocked)
        {
            Cursor.lockState = isLocked ? CursorLockMode.Locked : CursorLockMode.None;
        }

        public void Warp(Vector2 position)
        {
            Mouse.current.WarpCursorPosition(position);
        }

        public void SetActive(bool isActive)
        {
            _isActive = isActive;
        }
    }
}