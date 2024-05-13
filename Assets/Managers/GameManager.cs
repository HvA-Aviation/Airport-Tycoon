using System;
using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        
        public AudioManager AudioManager;
        public TaskManager TaskManager;

        private void Awake()
        {
            Instance = this;
        }
    }
}