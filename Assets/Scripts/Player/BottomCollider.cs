using UnityEngine;

namespace BubbleGum
{
    public class BottomCollider : MonoBehaviour
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
            Collider2D[] colliders = Physics2D.OverlapBoxAll( this.Center, this.Size, 0, layerMask );
            foreach (Collider2D collider in colliders)
            {
                if (collider.transform.position.y < this.transform.position.y)
                {
                    //Debug.Log( $"Bottom touch: {{{collider}}}", collider );
                    movement.Jump();
                }
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            Debug.Log( $"point: {collision.GetContact( 0 ).point}" );
        }

#if UNITY_EDITOR
        [Header("Editor only")]
        [SerializeField] private Color gizmosColor = Color.red;
        private void OnDrawGizmos()
        {
            Gizmos.color = this.gizmosColor;
            Gizmos.DrawWireCube( this.Center, this.Size );
        }
#endif
    }
}