using UnityEngine;

namespace AceInTheHole
{
    public class Block : MonoBehaviour, IInteractable
    {
        public int i, j;
        private int killed;
        private Movement movement;
        private Vector2 originalPos;

        public bool InPlace => !this.movement.enabled;
        public bool ShouldJumpOff => true;

        private void OnEnable()
        {
            if (this.movement)
            {
                this.movement.enabled = false;
            }
            this.transform.position = this.originalPos;
            this.killed = 0;
        }

        public void Initialize(int i, int j, Vector2 position)
        {
            this.i = i;
            this.j = j;

            this.originalPos = position;
            this.transform.position = this.originalPos;

            if (this.TryGetComponent( out Movement movement ))
            {
                this.movement = movement;
                movement.enabled = false;
            }
            else
            {
                Debug.LogError( "No 'Movement' component was found on this object", this );
            }
        }

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
        }

        [ContextMenu( nameof( Interact ) )]
        public void Interact()
        {
            this.CallDrop();
        }

        public void Hide()
        {
            this.gameObject.SetActive( false );
            PlayerScore.EnemiesKilled( killed );
        }

        public void GetKill()
        {
            killed++;
        }
    }
}