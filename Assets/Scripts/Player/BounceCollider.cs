using UnityEngine;

namespace AceInTheWhole
{
    public class BounceCollider : MonoBehaviour
    {
        [SerializeField] private Vector2 center = Vector2.zero;
        [SerializeField] private float width = 1, height = 1;
        [SerializeField] private LayerMask layerMask;

        private Movement movement;

        private Vector2 Size => new Vector2( this.width, this.height );
        private Vector2 Center => this.center + (Vector2)this.transform.position;

        private void Awake()
        {
            if (this.TryGetComponent( out Movement movement ))
            {
                this.movement = movement;
            }
            else
            {
                Debug.LogError( "Movement wasn't found", this );
            }
        }

        private void Update()
        {
            Collider2D collider = Physics2D.OverlapBox( this.Center, this.Size, 0, this.layerMask );
            if (collider)
            {
                //Debug.Log( $"Bottom touch: {{{collider}}}", collider );
                if (collider.TryGetComponent( out IInteractable interactable ))
                {
                    interactable.Interact();

                    if (interactable.ShouldJumpOff)
                    {
                        this.movement.Jump();
                    }
                }
                else
                {
                    this.movement.Jump();
                }
            }
        }

#if UNITY_EDITOR
        [Header("Editor only")]
        [SerializeField] private Color gizmosColor = Color.red;
        private void OnDrawGizmos()
        {
            if (!this.enabled)
            {
                return;
            }

            Gizmos.color = this.gizmosColor;
            Gizmos.DrawWireCube( this.Center, this.Size );
        }
#endif
    }
}