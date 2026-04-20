using System.Collections.Generic;
using UnityEngine;

namespace CrazyPawn
{
    public class Board : MonoBehaviour
    {
        private const float CellSize = 1.5f;

        private readonly List<Pawn> _pawns = new();
        private int _size;

        public void AddPawn(Pawn pawn)
        {
            _pawns.Add(pawn);
        }

        public void RemovePawn(Pawn pawn)
        {
            _pawns.Remove(pawn);
        }

        public IEnumerable<Pawn> GetPawns()
        {
            return _pawns;
        }

        public void BuildBoard(int size, BoardCell cellPrefab, Color blackCellColor, Color whiteCellColor)
        {
            _size = size;

            float half = GetHalfExtent();

            for (int row = 0; row < size; row++)
            {
                for (int col = 0; col < size; col++)
                {
                    Color color = (row + col) % 2 == 0 ? blackCellColor : whiteCellColor;
                    CreateBoardCell(cellPrefab, row, col, half, color);
                }
            }
        }

        public bool IsCenterOnBoard(Vector3 position)
        {
            float boardHalfExtent = GetHalfExtent();
            return Mathf.Abs(position.x) <= boardHalfExtent && Mathf.Abs(position.z) <= boardHalfExtent;
        }

        private float GetHalfExtent() => _size * CellSize * 0.5f;

        private void CreateBoardCell(BoardCell cellPrefab, int row, int col, float half, Color color)
        {
            BoardCell cell = Instantiate(cellPrefab, transform);
            float x = -half + col * CellSize + CellSize * 0.5f;
            float z = -half + row * CellSize + CellSize * 0.5f;
            cell.transform.localPosition = new Vector3(x, 0f, z);
            cell.transform.localScale = new Vector3(CellSize, CellSize, 1f);
            cell.SetColor(color);
        }
    }
}