using UnityEngine;

namespace AceInTheHole
{
    public class Movement : MonoBehaviour
    {
        [SerializeField] private bool enableHorizontal = true;

        private float bounce =  0;
        [Tooltip("This variable is used to indicate how many seconds the player horizontal input will be ignored right after bouncing off a wall")]
        [SerializeField] private float horizontalInputCooldown =  0.2f;
        private bool ignoreHoriontalInput = false;

        [SerializeField] private float gravity =  -0.1f;
        [SerializeField] private Vector2 maxSpeed = Vector2.one;
        private Vector2 speed;
        private SpriteRenderer spriteRenderer;

        private float HorizontalSpeed
        {
            get
            {
                float horizontalSpeed = this.bounce;
                if (!this.ignoreHoriontalInput)
                {
                    horizontalSpeed += Input.GetAxisRaw( "Horizontal" ) * this.maxSpeed.x;
                }

                return horizontalSpeed;
            }
        }

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
            this.speed = Vector2.zero;
        }

        //private void Start()
        //{
        //    GravitySwitch.GrabityFlipped += this.OnGravityFlipped;
        //}

        private void FixedUpdate()
        {
            this.speed.y = Mathf.Max( this.speed.y + this.gravity, -this.maxSpeed.y );
            this.bounce *= 0.9f;
        }
        private void Update()
        {
            float x = this.enableHorizontal ? this.HorizontalSpeed : 0;
            this.speed = new Vector2( x, this.speed.y );

            if (this.speed != Vector2.zero)
            {
                this.Move();
                this.CheckSpriteDirection();
            }
        }

        //private void OnDestroy()
        //{
        //    GravitySwitch.GrabityFlipped -= this.OnGravityFlipped;
        //}

        public void Jump(int horizontalValue = 0)
        {
            if (horizontalValue == 0)
            {
                this.speed.y = this.maxSpeed.y;
            }
            else
            {
                this.bounce = this.maxSpeed.x * 2 * horizontalValue;
                this.ignoreHoriontalInput = true;
                this.Invoke( nameof( AllowHorizontalInput ), this.horizontalInputCooldown );
            }
        }
        private void AllowHorizontalInput()
        {
            this.ignoreHoriontalInput = false;
        }

        private void Move()
        {
            Vector2 speed = this.speed * Time.deltaTime;

            //if (GravitySwitch.GrabityIsFlipped)
            //{
            //    speed.y *= -1;
            //}

            this.transform.Translate( speed );
        }

        private void CheckSpriteDirection()
        {
            if (this.speed.x == 0)
            {
                return;
            }

            this.spriteRenderer.flipX = this.speed.x < 0;
        }

        private void OnGravityFlipped()
        {
            this.speed.y = 0;

            BounceCollider[] collders = this.GetComponents<BounceCollider>();
            foreach (BounceCollider collder in collders)
            {
                collder.enabled = !collder.enabled;
            }
        }
    }
}