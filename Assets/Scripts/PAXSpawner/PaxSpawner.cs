using Features.Managers;
using System.Collections;
using UnityEngine;

public class PaxSpawner : MonoBehaviour
{
    [SerializeField] private float _defaultSpawnRate;

    private float _spawnRate;

    private void Awake() => _spawnRate = _defaultSpawnRate;

    private void Start() => StartCoroutine(SpawnPassengers());

    public void SetSpawnTime(float rateMultiplier, bool resetSpawnRate)
    {
        if (resetSpawnRate)
            _spawnRate = _defaultSpawnRate;
        else
            _spawnRate *= rateMultiplier;
    }

    private IEnumerator SpawnPassengers()
    {
        while (true)
        {
            yield return new WaitForSeconds(_spawnRate);

            GameManager.Instance.PaxManager.Pool.Get();
        }
    }
}
