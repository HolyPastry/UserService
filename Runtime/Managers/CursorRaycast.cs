
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Bakery
{
    public class CursorRaycast : MonoBehaviour, ICursorRaycast
    {
        public GameObject HoveredObject => _hoveredObject;
        public LayerMask InteractableLayer => _interactableLayerMask;

        [SerializeField] private LayerMask _interactableLayerMask;
        private GameObject _hoveredObject;
        private List<GameObject> _hoveredObjects;
        private Camera _camera;

        private void Awake()
        {
            _camera = Camera.main;
            User.Raycast = () => this;
        }

        void OnDestroy()
        {
            User.Raycast = User.UnregisterCursorRaycast;
        }

        void FixedUpdate()
        {
            if (_camera == null) _camera = Camera.main;

            if (TestIfCursorOverUI()) return;
            _hoveredObjects.Clear();
            TestIfCursorOverInteractable();
            if (_hoveredObject != null)
                _hoveredObjects.Add(_hoveredObject);
        }

        private void TestIfCursorOverInteractable()
        {
            if (!Physics.Raycast(_camera.ScreenPointToRay(User.Cursor().Position),
                    out RaycastHit hit, 1000f,
                    _interactableLayerMask))
            {
                if (_hoveredObject == null) return;
                User.Events.Cursor.OnExit?.Invoke(_hoveredObject);

                _hoveredObject = null;
                return;
            }

            if (hit.collider.gameObject == _hoveredObject) return;

            if (_hoveredObject != null)
                User.Events.Cursor.OnExit?.Invoke(_hoveredObject);

            _hoveredObject = hit.collider.gameObject;
            User.Events.Cursor.OnEnter?.Invoke(_hoveredObject);
        }

        private bool TestIfCursorOverUI()
        {
            if (!RaycastUtilities.PointerIsOverUI(User.Cursor().Position,
                        out _hoveredObjects))
                return false;

            if (_hoveredObjects[0] == _hoveredObject)
                return true;

            if (_hoveredObject != null)
                User.Events.Cursor.OnExit?.Invoke(_hoveredObject);

            _hoveredObject = _hoveredObjects[0];
            User.Events.Cursor.OnEnter?.Invoke(_hoveredObject);

            return true;
        }

        public IEnumerable<GameObject> HoveredObjects(Predicate<GameObject> predicate)
        {
            return _hoveredObjects.Where(obj => predicate(obj));
        }

        public bool IsHovering(GameObject obj)
        {
            return _hoveredObject == obj;
        }

        public bool IsOverUI(GameObject obj)
        {
            return _hoveredObject != null &&
                    _hoveredObject == obj &&
                    _hoveredObject.layer == LayerMask.NameToLayer("UI");
        }

        public bool IsOverUI()
        {
            return _hoveredObject != null &&
                _hoveredObject.layer == LayerMask.NameToLayer("UI");
        }
    }
}