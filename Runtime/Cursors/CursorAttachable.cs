
using System;
using System.Collections;
using UnityEngine;

namespace Bakery
{
    public class CursorAttachable : MonoBehaviour, ICursorAttachable
    {
        [SerializeField] private bool _attachOnlyIfHovered = true;

        public bool IsAttached { get; private set; }

        public void UpdatePosition(Vector2 position)
        {
            transform.position = position;
        }
        public void Attach()
        {
            if (_attachOnlyIfHovered && !User.Raycast().IsHovering(gameObject))
                return;
            User.Cursor().Attach(this);
            IsAttached = true;
        }

        public void Detach()
        {
            User.Cursor().Detach(this);
            IsAttached = false;
        }
    }
}
