
using System;
using UnityEngine;

namespace Bakery
{
    public class CursorDebugger : MonoBehaviour
    {
        [SerializeField] private bool _logPosition;
        void OnEnable()
        {
            User.Events.Cursor.OnEnter += OnEnter;
            User.Events.Cursor.OnExit += OnExit;
            User.Events.Cursor.OnSleep += OnSleep;
            User.Events.Cursor.OnWake += OnWake;
            User.Events.Cursor.OnAttach += OnAttach;
            User.Events.Cursor.OnDetach += OnDetach;

        }
        void OnDisable()
        {
            User.Events.Cursor.OnEnter -= OnEnter;
            User.Events.Cursor.OnExit -= OnExit;
            User.Events.Cursor.OnSleep -= OnSleep;
            User.Events.Cursor.OnWake -= OnWake;
            User.Events.Cursor.OnAttach -= OnAttach;
            User.Events.Cursor.OnDetach -= OnDetach;
        }

        private void OnDetach()
        {
            Debug.Log("Cursor detach");
            var attachedObjects = User.Cursor().AttachedObjects;
            foreach (var obj in attachedObjects)
                Debug.Log("Attached object: " + obj);
        }

        private void OnAttach()
        {
            Debug.Log("Cursor attach");
            var attachedObjects = User.Cursor().AttachedObjects;
            foreach (var obj in attachedObjects)
                Debug.Log("Attached object: " + obj);
        }

        private void OnSleep()
        {
            Debug.Log("Cursor sleep");
        }

        private void OnWake()
        {
            Debug.Log("Cursor wake");
        }

        private void OnExit(GameObject obj)
        {
            Debug.Log("Cursor exit: " + obj.name);
        }

        private void OnEnter(GameObject obj)
        {
            Debug.Log("Cursor enter: " + obj.name);
        }

        void Update()
        {
            if (_logPosition)
                Debug.Log(User.Cursor().Position);

        }

        void Start()
        {
            //noop
        }
    }
}
