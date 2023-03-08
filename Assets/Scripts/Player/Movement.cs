using UnityEngine;

namespace BubbleGum
{
    public class Movement : MonoBehaviour
    {
        [SerializeField] private float gravity =  -0.01f;

        [SerializeField] private Vector2 maxSpeed = Vector2.one;
        private Vector2 movement;

        public float? xMin, xMax;

        private void Awake()
        {
            Application.targetFrameRate = 60;
        }

        private void Update()
        {
            float x = Input.GetAxisRaw( "Horizontal" ) * this.maxSpeed.x;
            movement = new Vector2( x, this.movement.y );

            this.transform.Translate( movement * Time.deltaTime );
            //Mathf.Clamp()

            this.movement.y = Mathf.Max( this.movement.y + this.gravity, -this.maxSpeed.y );
        }

        public void Jump()
        {
            this.movement.y = this.maxSpeed.y;
        }
    }
}