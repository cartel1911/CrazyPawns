using UnityEngine;
using Random = UnityEngine.Random;

namespace CrazyPawn
{
    public class PawnSpawner
    {
        private readonly CrazyPawnSettings _settings;
        private readonly Pawn _pawnPrefab;
        private readonly PawnConnections _pawnConnections;

        public PawnSpawner(CrazyPawnSettings settings, Pawn pawnPrefab, PawnConnections pawnConnections)
        {
            _settings = settings;
            _pawnPrefab = pawnPrefab;
            _pawnConnections = pawnConnections;
        }

        public void InitialSpawn(Board board)
        {
            for (int i = 0; i < _settings.InitialPawnCount; i++)
            {
                Vector2 randomPosition = Random.insideUnitCircle * _settings.InitialZoneRadius;
                Vector3 p = new Vector3(randomPosition.x, 0, randomPosition.y);
                Pawn pawn = Object.Instantiate(_pawnPrefab, p, Quaternion.identity, board.transform);
                pawn.Init(_settings.DeleteMaterial, _pawnConnections);
                board.AddPawn(pawn);
            }
        }
    }
}