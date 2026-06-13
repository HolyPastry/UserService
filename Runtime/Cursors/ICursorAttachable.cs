using UnityEngine;

namespace Bakery
{
    public interface ICursorAttachable
    {
        void UpdatePosition(Vector2 position);
        bool IsAttached { get => false; }
        void Attach() { }
        void Detach() { }
    }
}