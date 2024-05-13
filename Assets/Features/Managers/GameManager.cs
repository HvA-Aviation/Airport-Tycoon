using UnityEngine;

namespace Features.Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        
        public AudioManager.Scripts.AudioManager AudioManager;
        public EventManager EventManager;
        public TaskManager TaskManager;

        private void Awake()
        {
            Instance = this;
        }
    }
}