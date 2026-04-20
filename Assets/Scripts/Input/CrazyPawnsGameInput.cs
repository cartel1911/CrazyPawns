using System.Collections.Generic;
using UnityEngine;

namespace CrazyPawn
{
    public class CrazyPawnsGameInput
    {
        private readonly BoardPanInputHandler _boardPanInputHandler;
        private readonly PawnDragInputHandler _pawnDragInputHandler;
        private readonly ConnectorLinkInputHandler _connectorLinkInputHandler;
        private readonly Camera _camera;

        public CrazyPawnsGameInput(
            Board board,
            PawnConnections connections,
            Material connectionLineMaterial,
            Material activeConnectorMaterial)
        {
            _camera = Camera.main;

            _boardPanInputHandler = new BoardPanInputHandler(_camera, board.transform);
            _pawnDragInputHandler = new PawnDragInputHandler(_camera, board);
            _connectorLinkInputHandler = new ConnectorLinkInputHandler(
                _camera,
                board,
                connections,
                connectionLineMaterial,
                activeConnectorMaterial);
        }

        public void Tick()
        {
            _boardPanInputHandler.HandleZoom();

            if (Input.GetMouseButtonDown(0))
            {
                OnPointerDown();
            }

            if (Input.GetMouseButton(0))
            {
                OnDrag();
            }

            if (Input.GetMouseButtonUp(0))
            {
                OnPointerUp();
            }
        }

        private void OnPointerDown()
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out RaycastHit hit, 500f))
            {
                _connectorLinkInputHandler.OnPointerDownMiss();
                _boardPanInputHandler.OnPointerDown();
                return;
            }

            if (hit.collider.TryGetComponent(out PawnConnector pawnConnector))
            {
                _connectorLinkInputHandler.OnPointerDownConnector(pawnConnector);
                return;
            }

            Pawn pawn = hit.collider.GetComponentInParent<Pawn>();
            if (pawn != null)
            {
                _pawnDragInputHandler.OnPointerDown(pawn);
                return;
            }

            _connectorLinkInputHandler.OnPointerDownMiss();
            _boardPanInputHandler.OnPointerDown();
        }

        private void OnDrag()
        {
            _pawnDragInputHandler.OnDrag();
            _connectorLinkInputHandler.OnDrag();
            _boardPanInputHandler.OnDrag();
        }

        private void OnPointerUp()
        {
            PawnConnector end = null;
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit uh, 500f))
            {
                end = uh.collider.GetComponent<PawnConnector>();
            }

            _connectorLinkInputHandler.OnPointerUp(end);
            _pawnDragInputHandler.OnPointerUp();
            _boardPanInputHandler.OnPointerUp();
        }
    }
}