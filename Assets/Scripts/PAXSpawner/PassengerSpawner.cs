using Features.Managers;
using System.Collections;
using UnityEngine;

public class PassengerSpawner : MonoBehaviour
{
    [SerializeField] private float _spawnRate;
    [SerializeField] private bool _canSpawnPassengerOnTime;
    public float SpawnRate => _spawnRate;

    private void Start()
    {
        StartCoroutine(SpawnPassengerOnTimer());
    }

    public void SpawnPassenger()
    {
        GameObject temp = GameManager.Instance.PoolManager.AllObjectPools[0].GetObject();
        temp.transform.position = transform.position;
    }

    public IEnumerator SpawnPassengerOnTimer()
    {
        while (_canSpawnPassengerOnTime)
        {
            yield return new WaitForSeconds(SpawnRate);

            SpawnPassenger();
        }
    }
}
