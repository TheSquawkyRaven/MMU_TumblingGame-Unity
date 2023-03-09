using UnityEngine;

namespace BubbleGum
{
    public class Block : MonoBehaviour
    {
        public int i, j;

        [SerializeField] private float width = 1, height = 1;
        [SerializeField] private LayerMask layerMask;

        private Movement movement;
        private bool hasBeenTouched = false;

        public bool InPlace => !this.movement.enabled;
        private Vector2 Center => (Vector2)this.transform.position;
        private Vector2 Size => new Vector2( this.width, this.height );

        public void Initialize(int i, int j)
        {
            this.i = i;
            this.j = j;

            if (this.TryGetComponent( out Movement movement ))
            {
                this.movement = movement;
                movement.enabled = false;
            }
            else
            {
                Debug.LogError( "No \"Movement\" component was found on this object", this );
            }
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

        private void FixedUpdate()
        {
            this.hasBeenTouched = false;
        }

        private void LateUpdate()
        {
            if (this.hasBeenTouched)
            {
                return;
            }

            Collider2D collider = Physics2D.OverlapBox( this.Center, this.Size, 0, this.layerMask );
            if (collider)
            {
                //Debug.Log( $"touched player: {{({this.i}, {this.j})}}", this );
                this.CallDrop();
                this.hasBeenTouched = true;
            }
        }

        [ContextMenu( nameof( CallDrop ) )]
        private void CallDrop()
        {
            BlockManager.CallDrop( this.i, this.j );
        }

        /// <summary>
        /// This method shall be only called from <see cref="BlockManager"/>
        /// </summary>
        [ContextMenu( nameof( Drop ) )]
        internal void Drop()
        {
            this.movement.enabled = true;
            this.Invoke( nameof( Hide ), 5 );
        }

        private void Hide()
        {
            this.gameObject.SetActive( false );
        }
    }
}