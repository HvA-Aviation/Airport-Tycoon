using UnityEngine;
using AudioManagerRef = Features.AudioManager.Scripts.AudioManager;

namespace Features.Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        [field: SerializeField] public AudioManagerRef AudioManager { get; private set; }
        [field: SerializeField] public EventManager EventManager { get; private set; }
        [field: SerializeField] public TaskManager TaskManager { get; private set; }
        [field: SerializeField] public BuildingManager BuildingManager { get; private set; }
        [field: SerializeField] public GridManager GridManager { get; private set; }
        [field: SerializeField] public QueueManager QueueManager { get; private set; }
        [field: SerializeField] public GameTimeManager GameTimeManager { get; private set; }

        private void Awake()
        {
            Instance = this;
        }
    }
}