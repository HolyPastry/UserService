
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bakery
{
    public interface ICursorRaycast
    {
        GameObject HoveredObject { get; }
        IEnumerable<GameObject> HoveredObjects(Predicate<GameObject> predicate);
        bool IsHovering(GameObject obj);
        bool IsOverUI(GameObject obj);
        bool IsOverUI();

        LayerMask InteractableLayer { get; }

    }
}