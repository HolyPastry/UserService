
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Bakery
{
    public class CursorUnityEvents : MonoBehaviour
    {
        public UnityEvent OnCursorEnter;
        public UnityEvent OnCursorExit;
        public UnityEvent OnCursorWake;
        public UnityEvent OnCursorSleep;
        public UnityEvent OnDeviceChange;

        void OnEnable()
        {
            User.Events.Cursor.OnEnter += CursorEnterHandler;
            User.Events.Cursor.OnExit += CursorExitHandler;
            User.Events.Cursor.OnSleep += CursorSleep;
            User.Events.Cursor.OnWake += CursorWake;
            User.Events.Device.OnChanged += DeviceChange;
        }



        void OnDisable()
        {
            User.Events.Cursor.OnEnter -= CursorEnterHandler;
            User.Events.Cursor.OnExit -= CursorExitHandler;
            User.Events.Cursor.OnSleep -= CursorSleep;
            User.Events.Cursor.OnWake -= CursorWake;
            User.Events.Device.OnChanged -= DeviceChange;
        }

        private void CursorEnterHandler(GameObject obj)
        {
            if (obj != gameObject) return;
            OnCursorEnter.Invoke();
        }
        private void CursorExitHandler(GameObject obj)
        {
            if (obj != gameObject) return;
            OnCursorExit.Invoke();
        }

        private void CursorSleep()
        {
            OnCursorSleep.Invoke();
        }

        private void CursorWake()
        {
            OnCursorWake.Invoke();
        }

        private void DeviceChange()
        {
            OnDeviceChange.Invoke();
        }
    }
}
