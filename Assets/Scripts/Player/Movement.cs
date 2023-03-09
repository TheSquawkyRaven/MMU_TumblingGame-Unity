using UnityEngine;

namespace BubbleGum
{
    public class Movement : MonoBehaviour
    {
        [SerializeField] private bool enableHorizontal = true;

        private float bounce =  0;
        [SerializeField] private float gravity =  -0.01f;
        [SerializeField] private Vector2 maxSpeed = Vector2.one;
        private Vector2 speed;

        public bool GrabityIsFlipped
        {
            get; set;
        }

        private void FixedUpdate()
        {
            this.speed.y = Mathf.Max( this.speed.y + this.gravity, -this.maxSpeed.y );
            this.bounce *= 0.9f;
        }
        private void Update()
        {
            float x = (Input.GetAxisRaw( "Horizontal" ) * this.maxSpeed.x) + this.bounce;
            this.speed = new Vector2( this.enableHorizontal ? x : 0, this.speed.y );
            this.Move();

            if (Input.GetKeyDown( KeyCode.Space ))
            {
                this.FlipGravity();
            }
        }

        public void Jump(int horizontalValue = 0)
        {
            if (horizontalValue == 0)
            {
                this.speed.y = this.maxSpeed.y;
            }
            else
            {
                this.bounce = this.maxSpeed.x * 2 * horizontalValue;
            }

            this.Move();
        }

        private void Move()
        {
            Vector2 speed = this.speed * Time.deltaTime;
            if (this.GrabityIsFlipped)
            {
                speed.y *= -1;
            }

            this.transform.Translate( speed );
        }

        [ContextMenu( nameof( FlipGravity ) )]
        public void FlipGravity()
        {
            this.speed = Vector2.zero;
            this.GrabityIsFlipped = !this.GrabityIsFlipped;

            BounceCollider[] collders = this.GetComponents<BounceCollider>();
            foreach (BounceCollider collder in collders)
            {
                collder.enabled = !collder.enabled;
            }
        }
    }
}