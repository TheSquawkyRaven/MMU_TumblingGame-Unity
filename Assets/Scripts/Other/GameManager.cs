using UnityEngine;

namespace AceInTheHole
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;

        public void Awake()
        {
            //Application.targetFrameRate = 60;

#if UNITY_EDITOR
            UnityEngine.Debug.unityLogger.logEnabled = true;
#else
            UnityEngine.Debug.unityLogger.logEnabled = false;
#endif

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
