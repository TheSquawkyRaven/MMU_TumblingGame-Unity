using UnityEngine;

namespace AceInTheHole
{
    public class GravitySwitch : MonoBehaviour, IInteractable
    {
        //public static event Action GrabityFlipped;

        public bool ShouldJumpOff => false;
        //public static bool GrabityIsFlipped
        //{
        //    get; private set;
        //}

        private float interactionCooldown = 1f;
        private new Collider2D collider;

        private void Awake()
        {
            //GravitySwitch.GrabityFlipped = null;
            if (this.TryGetComponent( out Collider2D collider ))
            {
                this.collider = collider;
            }
            else
            {
                Debug.LogError( "There is no 'Collider2D' component on this object", this );
            }
        }

        //private void Start()
        //{
        //    GravitySwitch.GrabityIsFlipped = false;
        //}

        private void FlipGravity()
        {
            //Debug.Log( "FlipGravity" );
            //GravitySwitch.GrabityIsFlipped = !GravitySwitch.GrabityIsFlipped;
            //GrabityFlipped?.Invoke();

            Block[] allBlocks = FindObjectsByType<Block>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            foreach (Block block in allBlocks)
            {
                if (!block.gameObject.activeSelf)
                {
                    block.gameObject.SetActive( true );
                }
            }

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
