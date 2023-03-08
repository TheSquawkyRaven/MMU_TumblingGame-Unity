using System.Collections.Generic;

using UnityEngine;

namespace BubbleGum
{
    public class BlockManager : MonoBehaviour
    {
        public List<List<Block>> blockMap;
        [SerializeField] private Vector2Int mapSize = new Vector2Int(6, 3);
        [SerializeField] private Vector2 offset = new Vector2(1, 1);

        [SerializeField] private Block blockPrefab;
        private Vector2 OriginalPos => this.transform.position;

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
                    block.Initialize( i, j );
                    this.blockMap[i].Add( block );

                    float yOffset = (-this.offset.y * j) - 0.5f;
                    float yPos = this.OriginalPos.y + yOffset;
                    block.transform.position = new Vector2( xPos, yPos );
                }
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
        }
#endif
    }
}