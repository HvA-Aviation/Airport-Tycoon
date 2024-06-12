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

        private void Start() => NormalTime();

        /// <summary>
        /// Call this function when you want to pause the game
        /// </summary>
        public void PauseGame() => TimeScale = 0;

        /// <summary>
        /// Call this function when you want to speed up the game
        /// </summary>
        /// <param name="speedMultiplier">The multiplier that decides how fast the game is going to go</param>
        public void SpeedUpGame(int speedMultiplier) => TimeScale = _defaultTimeScale * speedMultiplier;

        /// <summary>
        /// Call this function when you want the game to be played on the default time
        /// </summary>
        public void NormalTime() => TimeScale = _defaultTimeScale;
    }
}