using UnityEngine;

namespace CrazyPawn
{
    public class PawnDragInputHandler
    {
        private Pawn _dragPawn;
        private Vector3 _dragOffset;

        private readonly Board _board;
        private readonly Camera _camera;
        private readonly Plane _groundPlane = new(Vector3.up, Vector3.zero);

        public PawnDragInputHandler(Camera camera, Board board)
        {
            _camera = camera;
            _board = board;
        }

        public void OnPointerDown(Pawn pawn)
        {
            if (pawn == null)
            {
                return;
            }

            if (TryGetGroundPoint(Input.mousePosition, out Vector3 point))
            {
                _dragPawn = pawn;

                Vector3 p = pawn.transform.position;
                _dragOffset = new Vector3(p.x, 0f, p.z) - new Vector3(point.x, 0f, point.z);
            }
        }

        public void OnDrag()
        {
            if (_dragPawn == null)
            {
                return;
            }

            if (TryGetGroundPoint(Input.mousePosition, out Vector3 point))
            {
                Transform t = _dragPawn.transform;
                Vector3 next = new Vector3(point.x + _dragOffset.x, t.position.y, point.z + _dragOffset.z);
                t.position = next;
            }

            bool isDelete = !_board.IsCenterOnBoard(_dragPawn.transform.position);
            _dragPawn.SetDeleteVisual(isDelete);
        }

        public void OnPointerUp()
        {
            if (_dragPawn == null)
            {
                return;
            }

            if (!_board.IsCenterOnBoard(_dragPawn.transform.position))
            {
                _board.RemovePawn(_dragPawn);
                _dragPawn.DestroyPawn();
            }

            _dragPawn = null;
        }

        private bool TryGetGroundPoint(Vector2 mousePosition, out Vector3 world)
        {
            world = default;

            Ray ray = _camera.ScreenPointToRay(mousePosition);
            if (!_groundPlane.Raycast(ray, out float dist))
            {
                return false;
            }

            world = ray.GetPoint(dist);
            return true;
        }
    }
}