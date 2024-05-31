using Features.Managers;
using System.Collections;
using UnityEngine;

public class PassengerSpawner : MonoBehaviour
{
    [SerializeField] private bool _canSpawnPassengerOnTime;
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
        GameObject temp = GameManager.Instance.PoolManager.AllObjectPools[0].GetObject();
        temp.transform.position = transform.position;
    }

    /// <summary>
    /// This IEnumerator will spawn passengers on a time basis
    /// </summary>
    /// <returns></returns>
    public IEnumerator SpawnPassengerOnTimer()
    {
        while (_canSpawnPassengerOnTime)
        {
            yield return new WaitForSeconds(_spawnerManager.SpawnRate);

            SpawnPassenger();
        }
    }
}
