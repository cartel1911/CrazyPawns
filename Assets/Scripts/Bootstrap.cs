using UnityEngine;
using UnityEngine.Assertions;

namespace CrazyPawn
{
    public class Bootstrap : MonoBehaviour
    {
        private Board _board;
        private CrazyPawnsGameInput _gameInput;
        private PawnConnections _pawnConnections;

        [SerializeField] private CrazyPawnSettings _settings;
        [SerializeField] private Pawn _pawnPrefab;
        [SerializeField] private BoardCell _cellPrefab;
        [SerializeField] private Material _connectionLineMaterial;

        private void OnValidate()
        {
            Assert.IsNotNull(_settings);
            Assert.IsNotNull(_pawnPrefab);
            Assert.IsNotNull(_cellPrefab);
            Assert.IsNotNull(_connectionLineMaterial);
        }

        private void Awake()
        {
            _board = new GameObject("Board").AddComponent<Board>();
            _board.BuildBoard(
                _settings.CheckerboardSize,
                _cellPrefab,
                _settings.BlackCellColor,
                _settings.WhiteCellColor);

            _pawnConnections = new PawnConnections();
            
            var spawner = new PawnSpawner(_settings, _pawnPrefab, _pawnConnections);
            spawner.InitialSpawn(_board);

            _gameInput = new CrazyPawnsGameInput(
                _board,
                _pawnConnections,
                _connectionLineMaterial,
                _settings.ActiveConnectorMaterial);
        }

        private void Update()
        {
            _gameInput.Tick();
        }

        private void LateUpdate()
        {
            _pawnConnections.LateTick();
        }
    }
}