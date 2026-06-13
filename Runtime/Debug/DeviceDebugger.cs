
using UnityEngine;

namespace Bakery
{

    public class DebugInputChange : MonoBehaviour
    {
        void OnEnable()
        {
            User.Events.Device.OnChanged += OnDeviceChange;
        }
        void OnDisable()
        {
            User.Events.Device.OnChanged -= OnDeviceChange;
        }

        private void OnDeviceChange()
        {
            Debug.Log("Device changed: " + User.Device().InputScheme);
            Debug.Log("Device changed: " + User.Device().GamepadType);
        }
        void Start()
        {//noop}    
        }
    }
}
