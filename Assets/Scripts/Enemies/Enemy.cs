using UnityEngine;

namespace AceInTheHole
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private LayerMask layer;
        [SerializeField] private float startingSpeed = 0.1f, maxSpeed = 1f, speedAcceleration = 0.1f;
        private float currentSpeed;

        public float CurrentSpeed => this.IsDirectionRight ? currentSpeed : -currentSpeed;
        public bool IsDirectionRight
        {
            get; set;
        }

        private void OnEnable()
        {
            this.currentSpeed = this.startingSpeed;

            Vector2 pos = this.transform.position;
            pos.x = 0;
            this.transform.position = pos;
        }
        private void OnDisable()
        {
            // TODO: implement dying logic
            // Add s to player
        }

        public void FixedUpdate()
        {
            this.currentSpeed = Mathf.Clamp( this.currentSpeed + this.speedAcceleration, startingSpeed, maxSpeed );
        }
        public void Update()
        {
            this.transform.Translate( new Vector2( this.CurrentSpeed * Time.deltaTime, 0 ) );
            if (EnemySpawner.ShouldFlip( this ))
            {
                this.IsDirectionRight = !this.IsDirectionRight;
            }

            this.CheckCollision();
        }

        private void CheckCollision()
        {
            Collider2D collider = Physics2D.OverlapBox( this.transform.position, this.transform.localScale, 0, layer);
            if (collider)
            {
                //Debug.Log( $"touched an object", this );
                if (collider.gameObject.TryGetComponent( out PlayerScore score ))
                {
                    score.GameOver();
                }
                else if (collider.gameObject.TryGetComponent( out Block block ))
                {
                    if (block.transform.position.y > this.transform.position.y)
                    {
                        block.GetKill();
                        this.gameObject.SetActive( false );
                    }
                }
            }
        }
    }
}
