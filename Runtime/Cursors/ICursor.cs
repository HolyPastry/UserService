using UnityEngine;

namespace Bakery
{
    public interface ICursor
    {
        void SetCursor(CursorType cursorType);
        void SetVisibility(bool isVisible);
        void SetLockState(bool isLocked);
        void Warp(Vector2 position);
        void SetActive(bool isActive);
    }
}