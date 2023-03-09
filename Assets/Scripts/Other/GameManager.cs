using UnityEngine;

namespace BubbleGum
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;

        public void Awake()
        {
            Application.targetFrameRate = 60;

            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Debug.LogError( "Instance already exists", instance );
            }
        }
    }
}
