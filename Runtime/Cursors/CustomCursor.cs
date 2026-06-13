
using UnityEngine;

namespace Bakery
{
    public class CustomCursor : MonoBehaviour
    {
        [SerializeField] private CursorType _cursor;

        void OnValidate()
        {
            if (_cursor == null)
                Debug.LogWarning($"Cursor is not set for {gameObject.name} in {GetType().Name}", this);
        }
        public CursorType Cursor
        {
            get => _cursor; set => _cursor = value;
        }

        void Start()
        {
            //noop 
        }
    }
}
