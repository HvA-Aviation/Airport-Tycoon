using Features.Managers;
using System.Collections;
using UnityEngine;

public class PassengerSpawner : MonoBehaviour
{
    [SerializeField, Tooltip("If checked true, the passengers will spawn on a timely basis")] private bool _spawnPassengersOnTime;
    [SerializeField] private ObjectPool _passengerPool;
    [SerializeField] private PassengerSpawnManager _spawnerManager;
    private void Start()
    {
        StartCoroutine(SpawnPassengerOnTimer());
    }

    /// <summary>
    /// Call this function when you want to spawn an passenger
    /// </summary>
    public void SpawnPassenger()
    {
        GameObject temp = _passengerPool.Get();
        temp.SetActive(true);
        temp.transform.position = transform.position;
    }

    /// <summary>
    /// This IEnumerator will spawn passengers on a time basis
    /// </summary>
    /// <returns></returns>
    public IEnumerator SpawnPassengerOnTimer()
    {
        while (_spawnPassengersOnTime)
        {
            yield return new WaitForSeconds(_spawnerManager.SpawnRate);

            SpawnPassenger();
        }
    }
}
