using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Bakery
{
    public static class RaycastUtilities
    {
        public static bool PointerIsOverUI(Vector2 screenPos, out List<GameObject> hitObjects)
        {
            hitObjects = UIRaycast(ScreenPosToPointerData(screenPos));
            return hitObjects.Count > 0 && hitObjects[0].layer == LayerMask.NameToLayer("UI");
        }

        public static List<GameObject> UIRaycast(Vector2 screenPos)
            => UIRaycast(ScreenPosToPointerData(screenPos));

        public static List<GameObject> UIRaycast()
            => UIRaycast(User.Cursor().Position);

        static List<GameObject> UIRaycast(PointerEventData pointerData)
        {
            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            return results.ConvertAll(result => result.gameObject);
        }



        public static Vector2 GetMouseUIPosition()
            => ScreenPosToPointerData(User.Cursor().Position).position;

        static PointerEventData ScreenPosToPointerData(Vector2 screenPos)
            => new(EventSystem.current) { position = screenPos };

        internal static bool IsObjectUnderPointer(GameObject gameObject)
        {
            List<GameObject> objList = UIRaycast(User.Cursor().Position);
            if (objList.Count == 0) return false;

            foreach (var o in objList)
                if (o.CompareTag(gameObject.tag) && ReferenceEquals(o, gameObject))
                    return true;

            return false;
        }




        // internal static void GetHitPosition(BagItemComponent itemBeingPlaced, out Vector3 worldPosition)
        // {
        //     var pointerData = ScreenPosToPointerData(InputServices.GetMousePosition());
        //     var results = new List<RaycastResult>();
        //     EventSystem.current.RaycastAll(pointerData, results);

        //     if (results.Count == 0)
        //     {
        //         worldPosition = InputServices.GetMousePosition();
        //         return;
        //     }
        //     worldPosition = results[0].worldPosition;
        // }
    }
}
