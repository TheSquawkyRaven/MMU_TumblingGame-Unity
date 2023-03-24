using System.Collections;

using UnityEngine;

namespace AceInTheHole
{
    public class BounceCollider : MonoBehaviour
    {
        [SerializeField] private Vector2 center = Vector2.zero;
        [SerializeField] private float width = 1, height = 1;
        [SerializeField] private LayerMask layerMask;

        [SerializeField] private float resetSpeed;
        private Movement movement;
        private Vector2 originalPos;

        private Vector2 Size => new Vector2( this.width, this.height );
        private Vector2 Center => this.center + (Vector2)this.transform.position;

        private void Awake()
        {
            this.movement = this.GetComponent<Movement>();
            if (this.movement == null)
            {
                Debug.LogError( "Movement wasn't found", this );
            }
        }

        private void Start()
        {
            this.originalPos = this.transform.position;
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
                        this.CallJump();
                    }
                    else
                    {
                        if (collider.GetComponent<GravitySwitch>() != null)
                        {
                            this.movement.enabled = false;
                            _ = this.StartCoroutine( this.ResetPosition() );
                        }
                    }
                }
                else
                {
                    this.CallJump();
                }
            }
        }

        private void CallJump()
        {
            this.movement.Jump();
            this.enabled = false;
            this.Invoke( nameof( this.JumpCooldown ), 1f );
        }
        private void JumpCooldown()
        {
            this.enabled = true;
        }

        private IEnumerator ResetPosition()
        {
            while (Mathf.Abs( this.originalPos.y - this.transform.position.y ) > 0.001f)
            {
                float yPos = Mathf.MoveTowards(this.transform.position.y, this.originalPos.y, this.resetSpeed);
                this.transform.position = new Vector2( this.transform.position.x, yPos );
                yield return null;
            }

            this.movement.enabled = true;
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