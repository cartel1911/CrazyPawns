using Unity.Collections;
using UnityEngine;

namespace CrazyPawn
{
    [RequireComponent(typeof(PawnDeletePaintComponent))]
    public class Pawn : MonoBehaviour
    {
        [ReadOnly] [SerializeField] private PawnDeletePaintComponent _deletePaintComponent;

        public PawnConnector[] Connectors { get; private set; }

        public void Init(Material deleteMaterial, PawnConnections pawnConnections)
        {
            Connectors = GetComponentsInChildren<PawnConnector>();
            foreach (PawnConnector c in Connectors)
            {
                c.Init(this, pawnConnections);
            }

            _deletePaintComponent.Init(deleteMaterial);
        }

        public void SetDeleteVisual(bool isDelete)
        {
            _deletePaintComponent.SetVisualByDeleteState(isDelete);
        }

        public void DestroyPawn()
        {
            foreach (PawnConnector c in Connectors)
            {
                c.Dispose();
            }

            Destroy(gameObject);
        }

        private void OnValidate()
        {
            _deletePaintComponent = _deletePaintComponent != null
                ? _deletePaintComponent
                : GetComponent<PawnDeletePaintComponent>();
        }
    }
}