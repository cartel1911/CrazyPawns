using System;
using Unity.Collections;
using UnityEngine;

namespace CrazyPawn
{
    [RequireComponent(typeof(MeshRenderer))]
    public class PawnConnector : MonoBehaviour, IDisposable
    {
        private PawnConnections _pawnConnections;
        private Material _defaultMaterial;

        [ReadOnly] [SerializeField] private MeshRenderer _renderer;

        public Pawn Owner { get; private set; }

        public void Init(Pawn owner, PawnConnections pawnConnections)
        {
            Owner = owner;
            _pawnConnections = pawnConnections;
            _defaultMaterial = _renderer.sharedMaterial;
        }

        public void Dispose()
        {
            _pawnConnections.RemoveConnections(this);
        }

        public void SetMaterial(Material material)
        {
            _renderer.material = material;
        }

        public void RestoreMaterial()
        {
            _renderer.material = _defaultMaterial;
        }

        private void OnValidate()
        {
            _renderer = _renderer != null ? _renderer : GetComponent<MeshRenderer>();
        }
    }
}