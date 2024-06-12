using UnityEngine;

namespace Features.Managers
{
    public class GameTimeManager : MonoBehaviour
    {
        [SerializeField] private float _defaultTimeScale = 1;

        public float TimeScale { get; private set; }        

        public float DeltaTime
        {
            get { return Time.deltaTime * TimeScale; }
        }

        public void PauseGame() => TimeScale = 0;

        public void SpeedUpGame(int speedMultiplier) => TimeScale = _defaultTimeScale * speedMultiplier;

        public void NormalTime() => TimeScale = 1;
    }
}