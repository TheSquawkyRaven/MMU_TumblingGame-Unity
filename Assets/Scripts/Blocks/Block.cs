using UnityEngine;

namespace BubbleGum
{
    public class Block : MonoBehaviour
    {
        public int i, j;
        private Movement movement;

        public bool InPlace => !this.movement.enabled;

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