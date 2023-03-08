using UnityEngine;

namespace BubbleGum
{
    public class Block : MonoBehaviour
    {
        public int i, j;

        public void Initialize(int i, int j)
        {
            this.i = i;
            this.j = j;
        }
    }
}