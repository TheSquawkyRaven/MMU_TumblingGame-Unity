using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace AceInTheHole
{
    public class EnemySpawner : MonoBehaviour
    {
        public static EnemySpawner instance;

        [Range(1f, 20f)]
        [SerializeField] private float borderDistance = 1f;
        [Range(1, 5)]
        [SerializeField] private int maxEnemies = 2;

        [SerializeField] private Pooler<Enemy> enemies = new Pooler<Enemy>();

        public static float RightBorder => instance.borderDistance;
        public static float LeftBorder => -instance.borderDistance;
        private float RandomPosX => Random.Range( LeftBorder, RightBorder );

        public void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Debug.LogError( "Instance already exists", instance );
            }
        }

        private void Start()
        {
            enemies.Initialise( this.SpawnNewInstance );
            this.InvokeRepeating( nameof( SpawnEnemy ), 2, 2 );
        }

        private void OnDestroy()
        {
            this.CancelInvoke( nameof( SpawnEnemy ) );
        }

        private void SpawnEnemy()
        {
            IEnumerable<Enemy> livingEnemies = enemies.pool.Where( x => x.gameObject.activeSelf ).ToArray();
            //Debug.Log( "livingEnemies.Count(): " + livingEnemies.Count() );
            bool shouldSpawn = livingEnemies.Count() < maxEnemies;
            //Debug.Log( "shouldSpawn: " + shouldSpawn );
            if (shouldSpawn)
            {
                Enemy enemy = enemies.Get();
                enemy.transform.position = new Vector2( this.RandomPosX, this.transform.position.y );
            }
        }

        public static bool ShouldFlip(Enemy enemy)
        {
            return enemy.IsDirectionRight
                ? enemy.transform.position.x >= RightBorder
                : enemy.transform.position.x <= LeftBorder;
        }
        private Enemy SpawnNewInstance(Enemy prefab)
        {
            return Instantiate( prefab, new Vector2( this.RandomPosX, this.transform.position.y ), Quaternion.identity );
        }

#if UNITY_EDITOR
        [Header("Editor only")]
        [Range(0.5f, 10.0f)]
        [SerializeField] private float radius = 1.0f;
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere( new Vector2( this.borderDistance, this.transform.position.y ), this.radius );
            Gizmos.DrawWireSphere( new Vector2( -this.borderDistance, this.transform.position.y ), this.radius );
        }
#endif
    }
}
