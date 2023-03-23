using System;

using UnityEngine;

namespace AceInTheWhole
{
    public class WorldGravity : MonoBehaviour, IInteractable
    {
        public static event Action GrabityFlipped;

        public bool ShouldJumpOff => false;
        public static bool GrabityIsFlipped
        {
            get; private set;
        }

        [SerializeField] private float interactionCooldown = 2f;
        private new Collider2D collider;

        private void Awake()
        {
            WorldGravity.GrabityFlipped = null;
            if (this.TryGetComponent( out Collider2D collider ))
            {
                this.collider = collider;
            }
            else
            {
                Debug.LogError( "There is no 'Collider2D' component on this object", this );
            }
        }

        private void Start()
        {
            WorldGravity.GrabityIsFlipped = false;
        }

        private void FlipGravity()
        {
            //Debug.Log( "FlipGravity" );
            WorldGravity.GrabityIsFlipped = !WorldGravity.GrabityIsFlipped;
            GrabityFlipped?.Invoke();

            this.collider.enabled = false;
            this.Invoke( nameof( Cooldown ), this.interactionCooldown );
        }
        private void Cooldown()
        {
            this.collider.enabled = true;
        }

        [ContextMenu( nameof( Interact ) )]
        public void Interact()
        {
            this.FlipGravity();
        }
    }
}
