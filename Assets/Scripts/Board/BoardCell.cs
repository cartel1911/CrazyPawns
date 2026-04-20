using Unity.Collections;
using UnityEngine;

namespace CrazyPawn
{
    [RequireComponent(typeof(MeshRenderer))]
    public class BoardCell : MonoBehaviour
    {
        [ReadOnly] [SerializeField] private MeshRenderer _renderer;

        private void OnValidate()
        {
            _renderer = _renderer != null ? _renderer : GetComponent<MeshRenderer>();
        }

        public void SetColor(Color color)
        {
            _renderer.material.color = color;
        }
    }
}