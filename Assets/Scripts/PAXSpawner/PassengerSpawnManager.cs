using UnityEngine;

public class PassengerSpawnManager : MonoBehaviour
{
    [SerializeField] private float _spawnRate;

    public float SpawnRate { get; private set; }

    private void Awake() => SpawnRate = _spawnRate;

    /// <summary>
    /// This function will set the spawnrate of the passengers to a new value
    /// </summary>
    /// <param name="newSpawnRate">The new spawnrate of the passengers</param>
    public void SetSpawnRate(float newSpawnRate) => SpawnRate = newSpawnRate;


}
