using UnityEngine;

namespace Bakery
{
    [CreateAssetMenu(fileName = "CursorType", menuName = "Bakery/User/CursorType", order = 0)]
    public class CursorType : ScriptableObject
    {
        public Texture2D Texture;
        public Vector2 Hotspot;
    }
}