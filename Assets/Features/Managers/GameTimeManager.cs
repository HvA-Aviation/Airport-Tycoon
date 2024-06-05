using UnityEngine;

namespace Features.Managers
{
    public class GameTimeManager : MonoBehaviour
    {
        [SerializeField] private float _timeScale = 1;

        public float TimeScale
        {
            get { return _timeScale; }
            set { _timeScale = value; }
        }

        public float DeltaTime
        {
            get { return Time.deltaTime * _timeScale; }
        }
    }
}