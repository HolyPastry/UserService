
using UnityEngine;

namespace Bakery
{
    public class CustomCursor : MonoBehaviour
    {
        [SerializeField] private CursorType _cursor;
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
