using System.Collections.Generic;

using UnityEngine;

namespace AceInTheHole
{
    public class BlockManager : MonoBehaviour
    {
        public static BlockManager instance;

        public List<List<Block>> blockMap;
        [SerializeField] private Vector2Int mapSize = new Vector2Int(6, 3);
        [SerializeField] private Vector2 offset = new Vector2(1, 1);
        [SerializeField] private float underGround = -5f;

        [SerializeField] private Block blockPrefab;
        private Vector2 OriginalPos => this.transform.position;

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
            float halfSize = this.mapSize.x * 0.5f;

            this.blockMap = new List<List<Block>>( capacity: this.mapSize.x );

            for (int i = 0; i < this.mapSize.x; i++)
            {
                this.blockMap.Add( new List<Block>( capacity: this.mapSize.y ) );

                float xOffset = this.offset.x * ( i - halfSize + 0.5f);
                float xPos = this.OriginalPos.x + xOffset;

                for (int j = 0; j < this.mapSize.y; j++)
                {
                    Block block = Instantiate( this.blockPrefab, Vector3.zero, Quaternion.identity );

                    float yOffset = (-this.offset.y * j) - 0.5f;
                    float yPos = this.OriginalPos.y + yOffset;

                    block.Initialize( i, j, new Vector2( xPos, yPos ) );
                    this.blockMap[i].Add( block );
                }
            }
        }

        public void Update()
        {
            foreach (List<Block> blockColumn in this.blockMap)
            {
                foreach (Block block in blockColumn)
                {
                    if (block.gameObject.activeSelf && !block.InPlace && block.transform.position.y < underGround)
                    {
                        block.Hide();
                    }
                }
            }
        }

        public static void CallDrop(int i, int j)
        {
            List<Block> blockColumn = instance.blockMap[i];

            if (j == ( blockColumn.Count - 1 ) || !blockColumn[j + 1].InPlace)
            {
                blockColumn[j].Drop();
            }
            else
            {
                BlockManager.CallDrop( i, j + 1 );
            }
        }

#if UNITY_EDITOR
        [Header("Editor only")]
        [Range(0.5f, 10.0f)]
        [SerializeField] private float radius = 1.0f;
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere( this.OriginalPos, this.radius );

            Gizmos.color = Color.red;
            Gizmos.DrawWireCube( Vector2.up * this.underGround, Vector2.one * this.radius );
        }
#endif
    }
}