using Bakery;

using UnityEngine;

namespace CrawDad
{
    public class RaycastDebugger : MonoBehaviour
    {

        void OnEnable()
        {
            User.Events.Cursor.OnEnter += OnCursorEnter;
            User.Events.Cursor.OnExit += OnCursorExit;
        }

        void OnDisable()
        {
            User.Events.Cursor.OnEnter -= OnCursorEnter;
            User.Events.Cursor.OnExit -= OnCursorExit;
        }

        private void OnCursorEnter(GameObject obj)
        {
            Debug.Log("Cursor Entered " + obj.name);
        }
        private void OnCursorExit(GameObject obj)
        {
            Debug.Log("Cursor Exited " + obj.name);
        }
    }
}