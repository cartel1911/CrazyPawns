using UnityEngine;

namespace CrazyPawn
{
    public class PawnDeletePaintComponent : MonoBehaviour
    {
        private MeshRenderer[] _renderers;
        private Material[] _materialSnapshots;

        private Material _deleteMaterial;

        public void Init(Material deleteMaterial)
        {
            _deleteMaterial = deleteMaterial;

            _renderers = GetComponentsInChildren<MeshRenderer>();
            _materialSnapshots = new Material[_renderers.Length];
            for (int i = 0; i < _renderers.Length; i++)
            {
                _materialSnapshots[i] = _renderers[i].material;
            }
        }

        public void SetVisualByDeleteState(bool isDelete)
        {
            foreach (MeshRenderer r in _renderers)
            {
                r.material = _deleteMaterial;
            }

            for (int i = 0; i < _renderers.Length; i++)
            {
                _renderers[i].material = isDelete ? _deleteMaterial : _materialSnapshots[i];
            }
        }
    }
}