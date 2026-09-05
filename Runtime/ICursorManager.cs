using System.Collections.Generic;
using UnityEngine;

namespace Bakery
{
    public interface ICursorManager
    {

        Vector2 Position { get; set; }

        IEnumerable<ICursorAttachable> AttachedObjects { get; }
        void Override(CursorType type);
        void RemoveOverride();

        void Attach(ICursorAttachable transform);
        void Detach(ICursorAttachable transform);

        void PutToSleep();
        void WakeUp();

        void SetVisibility(bool isOn);
        void Unlock(bool unlock);

        void StorePosition();
        void RecallPosition();
        void ResetPosition();


    }
}