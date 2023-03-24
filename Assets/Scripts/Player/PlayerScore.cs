using TMPro;

using UnityEngine;

namespace AceInTheHole
{
    public class PlayerScore : MonoBehaviour
    {
        public static PlayerScore instance;
        private static int s;
        public static int Score
        {
            get => s; set
            {
                s = value;
                instance.scoreText.text = "Score " + s;
            }
        }

        [SerializeField] private TextMeshProUGUI scoreText;

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
            Score = 0;
        }

        public static void EnemiesKilled(int count)
        {
            //Debug.Log( "Killed: " + count );
            if (count <= 0)
            {
                return;
            }
            else if (count == 1)
            {
                Score += count;
            }
            else
            {
                Score += (int)Mathf.Pow( count, 2 );
            }
        }

        public void GameOver()
        {
            this.gameObject.SetActive( false );
        }
    }
}
