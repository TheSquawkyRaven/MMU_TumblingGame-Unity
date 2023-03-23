using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace BubbleGum
{
    
    public class Score : MonoBehaviour
    {
        public int Scores = 0;
        public TextMeshProUGUI Scoretext;
        public TextMeshProUGUI FinalScoretext;
        void Update()
        {
            //if(enemy dead? or block and enemy is collide)
            {
                Scores += 1;
                Scoretext.text = Scores.ToString();
            }

            FinalScoretext.text = Scores.ToString();
        }
    }
}
