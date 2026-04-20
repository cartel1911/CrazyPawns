using UnityEngine;

namespace CrazyPawn
{
    public class BoardPanInputHandler
    {
        private const float ZoomStep = 2f;
        private const float PanScreenToWorld = 0.02f;

        private readonly Camera _camera;
        private readonly Transform _board;

        private bool _panning;
        private Vector2 _lastPanScreen;

        private bool _appHadFocusLastFrame = true;
        
        public BoardPanInputHandler(Camera camera, Transform boardTransform)
        {
            _camera = camera;
            _board = boardTransform;
        }

        public void HandleZoom()
        {
            if (!Application.isFocused)
            {
                _appHadFocusLastFrame = false;
                return;
            }
            
            if (!_appHadFocusLastFrame)
            {
                _appHadFocusLastFrame = true;
                return;
            }

            float scroll = Input.mouseScrollDelta.y;
            if (!Mathf.Approximately(scroll, 0f))
            {
                _camera.transform.position += _camera.transform.forward * (scroll * ZoomStep);
            }
        }

        public void OnPointerDown()
        {
            _panning = true;
            _lastPanScreen = Input.mousePosition;
        }

        public void OnDrag()
        {
            if (!_panning)
            {
                return;
            }

            Vector2 delta = (Vector2)Input.mousePosition - _lastPanScreen;
            _lastPanScreen = Input.mousePosition;

            _camera.transform.position -= (_board.right * delta.x + _board.forward * delta.y) * PanScreenToWorld;
        }

        public void OnPointerUp()
        {
            _panning = false;
        }
    }
}