using JetBrains.Annotations;
using UnityEngine;

namespace CrazyPawn
{
    public enum ConnectionState
    {
        None,
        Drag,
        WaitingSecondClick
    }

    public class ConnectorLinkInputHandler
    {
        private const float ConnectionDragEndThresholdPx = 10f;

        private ConnectionState _connectionState;

        private Vector2 _inputDownPosition;
        [CanBeNull] private PawnConnector _selectedConnector;
        [CanBeNull] private LineRenderer _connectLineRenderer;

        private readonly Camera _camera;
        private readonly Board _board;
        private readonly PawnConnections _connections;
        private readonly Material _connectionLineMaterial;
        private readonly Material _activeConnectorMaterial;

        public ConnectorLinkInputHandler(
            Camera camera,
            Board board,
            PawnConnections connections,
            Material connectionLineMaterial,
            Material activeConnectorMaterial)
        {
            _connections = connections;
            _camera = camera;
            _board = board;
            _connectionLineMaterial = connectionLineMaterial;
            _activeConnectorMaterial = activeConnectorMaterial;
        }

        public void OnPointerDownConnector(PawnConnector connector)
        {
            if (_connectionState == ConnectionState.None)
            {
                _selectedConnector = connector;
                _connectionState = ConnectionState.Drag;
                _inputDownPosition = Input.mousePosition;

                HighlightConnectors(connector);

                _connectLineRenderer = CreateLineRenderer();
            }
            else if (_connectionState == ConnectionState.WaitingSecondClick)
            {
                if (PawnConnections.IsValidConnection(_selectedConnector, connector))
                {
                    _connections.TryAddConnection(_connectLineRenderer, _selectedConnector, connector);
                }
                else
                {
                    DestroyConnectionLine();
                }
                
                _connectLineRenderer = null;
                EndConnectionMode();
            }
        }

        public void OnDrag()
        {
            if (_connectionState != ConnectionState.Drag || _selectedConnector == null)
            {
                return;
            }

            Vector3 start = _selectedConnector.transform.position;
            Vector3 end = start;
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            var plane = new Plane(Vector3.up, start);
            if (plane.Raycast(ray, out float dist))
            {
                end = ray.GetPoint(dist);
            }

            if (_connectLineRenderer != null)
            {
                _connectLineRenderer.SetPosition(0, start);
                _connectLineRenderer.SetPosition(1, end);
            }
        }

        public void OnPointerUp([CanBeNull] PawnConnector connectorUnderCursor)
        {
            if (_connectionState == ConnectionState.Drag)
            {
                if (connectorUnderCursor != null &&
                    PawnConnections.IsValidConnection(_selectedConnector, connectorUnderCursor))
                {
                    _connections.TryAddConnection(_connectLineRenderer, _selectedConnector, connectorUnderCursor);
                    _connectLineRenderer = null;
                    EndConnectionMode();
                }
                else
                {
                    if (Vector2.Distance(_inputDownPosition, Input.mousePosition) >= ConnectionDragEndThresholdPx)
                    {
                        EndConnectionMode();
                        DestroyConnectionLine();
                    }
                    else
                    {
                        _connectionState = ConnectionState.WaitingSecondClick;
                    }
                }
            }
        }

        public void OnPointerDownMiss()
        {
            if (_connectionState == ConnectionState.WaitingSecondClick)
            {
                DestroyConnectionLine();
                EndConnectionMode();
            }
        }

        private void EndConnectionMode()
        {
            foreach (Pawn p in _board.GetPawns())
            {
                foreach (PawnConnector c in p.Connectors)
                {
                    c.RestoreMaterial();
                }
            }

            _selectedConnector = null;
            _connectionState = ConnectionState.None;
        }

        private void HighlightConnectors(PawnConnector selectedConnector)
        {
            Pawn sourcePawn = selectedConnector.Owner;

            foreach (Pawn p in _board.GetPawns())
            {
                if (p == sourcePawn)
                {
                    continue;
                }

                foreach (PawnConnector c in p.Connectors)
                {
                    if (!_connections.HasConnectionBetween(selectedConnector, c))
                    {
                        c.SetMaterial(_activeConnectorMaterial);
                    }
                }
            }
        }

        private LineRenderer CreateLineRenderer()
        {
            var go = new GameObject("Connections");
            LineRenderer lr = go.AddComponent<LineRenderer>();
            lr.transform.SetParent(_board.transform, false);
            lr.material = _connectionLineMaterial;
            lr.positionCount = 2;
            lr.widthMultiplier = 0.07f;
            lr.useWorldSpace = true;
            lr.numCapVertices = 2;
            lr.numCornerVertices = 2;
            lr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            lr.receiveShadows = false;
            lr.startColor = Color.white;
            lr.endColor = Color.white;

            return lr;
        }

        private void DestroyConnectionLine()
        {
            if (_connectLineRenderer != null)
            {
                Object.Destroy(_connectLineRenderer.gameObject);
                _connectLineRenderer = null;
            }
        }
    }
}