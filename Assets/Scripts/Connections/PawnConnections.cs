using System.Collections.Generic;
using UnityEngine;

namespace CrazyPawn
{
    public struct ConnectionVisual
    {
        public LineRenderer Line;
        public PawnConnector A;
        public PawnConnector B;
    }

    public class PawnConnections
    {
        private readonly List<ConnectionVisual> _connections = new();
        private readonly HashSet<long> _existingPairs = new();

        public bool HasConnectionBetween(PawnConnector a, PawnConnector b)
        {
            return _existingPairs.Contains(PairKey(a.GetInstanceID(), b.GetInstanceID()));
        }

        public void TryAddConnection(LineRenderer lineRenderer, PawnConnector a, PawnConnector b)
        {
            int idA = a.GetInstanceID();
            int idB = b.GetInstanceID();
            long key = PairKey(idA, idB);
            if (_existingPairs.Add(key))
            {
                _connections.Add(new ConnectionVisual { Line = lineRenderer, A = a, B = b });
            }
        }

        public void RemoveConnections(PawnConnector end)
        {
            for (int i = _connections.Count - 1; i >= 0; i--)
            {
                ConnectionVisual c = _connections[i];
                if (c.A != end && c.B != end)
                {
                    continue;
                }

                _existingPairs.Remove(PairKey(c.A.GetInstanceID(), c.B.GetInstanceID()));

                if (c.Line != null)
                {
                    Object.Destroy(c.Line.gameObject);
                }

                _connections.RemoveAt(i);
            }
        }

        public void LateTick()
        {
            foreach (ConnectionVisual c in _connections)
            {
                c.Line.SetPosition(0, c.A.transform.position);
                c.Line.SetPosition(1, c.B.transform.position);
            }
        }

        public static bool IsValidConnection(PawnConnector a, PawnConnector b)
        {
            return a.Owner != b.Owner;
        }

        private static long PairKey(int a, int b)
        {
            if (a > b)
            {
                (a, b) = (b, a);
            }

            return ((long)a << 32) | (uint)b;
        }
    }
}