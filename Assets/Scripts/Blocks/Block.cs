using System.Collections;

using UnityEngine;

namespace AceInTheHole
{
    public class Block : MonoBehaviour, IInteractable
    {
        public enum Level { Top, Mid, Bot };

        [SerializeField] private float resetSpeed = 1f;
        private int i, j;
        private int killed;
        private Movement movement;
        private Vector2 originalPos;
        private SpriteRenderer spriteRenderer;
        [SerializeField] private Sprite[] sprites;

        private bool goingUp = false;
        public bool GoingUp => goingUp;

        public bool InPlace => !this.movement.enabled;
        public bool ShouldJumpOff => true;

        private void Awake()
        {
            this.spriteRenderer = this.GetComponentInChildren<SpriteRenderer>();
            if (this.spriteRenderer == null)
            {
                Debug.LogError( "There is no 'SpriteRenderer' component on this object", this );
            }
        }

        private void OnEnable()
        {
            //this.transform.position = this.originalPos;
            this.killed = 0;

            if (this.movement)
            {
                this.movement.enabled = false;
                this.goingUp = true;
                _ = this.StartCoroutine( this.ResetPosition() );
            }
        }

        private IEnumerator ResetPosition()
        {
            while (Mathf.Abs( this.originalPos.y - this.transform.position.y ) > 0.001f)
            {
                float yPos = Mathf.MoveTowards(this.transform.position.y, this.originalPos.y, this.resetSpeed);
                this.transform.position = new Vector2( this.transform.position.x, yPos );
                yield return null;
            }

            this.goingUp = false;
        }

        public void Initialize(int i, int j, Vector2 position, Level level)
        {
            this.i = i;
            this.j = j;

            this.spriteRenderer.sprite = this.sprites[(int)level];

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
        internal void Drop()
        {
            this.movement.enabled = true;
        }

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