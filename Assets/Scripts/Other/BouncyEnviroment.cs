using UnityEngine;

namespace BubbleGum
{
    public class BouncyEnviroment : MonoBehaviour
    {
        [SerializeField] private LayerMask playerLayer;

        private void Update()
        {
            Collider2D collider = Physics2D.OverlapBox( this.transform.position, this.transform.localScale, 0, playerLayer);
            if (collider)
            {
                //Debug.Log( $"touched player", this );
                if (collider.gameObject.TryGetComponent( out Movement movement ))
                {
                    if (this.transform.position.x > collider.transform.position.x)
                    {
                        movement.Jump( -1 );
                    }
                    else
                    {
                        movement.Jump( 1 );
                    }
                }
            }
        }


#if UNITY_EDITOR
        [Header("Editor only")]
        [SerializeField] private Color gizmosColor = Color.red;
        private void OnDrawGizmos()
        {
            Gizmos.color = this.gizmosColor;
            Gizmos.DrawWireCube( this.transform.position, this.transform.localScale );
        }
#endif

        //private void OnCollisionEnter2D(Collision2D collision)
        //{
        //    if (collision != null)
        //    {
        //        if (collision.gameObject.TryGetComponent( out Movement movement ))
        //        {
        //            if (this.transform.position.x > collision.transform.position.x)
        //            {
        //                movement.Jump( -1 );
        //            }
        //            else
        //            {
        //                movement.Jump( 1 );
        //            }
        //        }
        //    }
        //}
    }
}
