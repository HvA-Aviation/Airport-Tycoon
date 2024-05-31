using System;
using System.Collections;
using Implementation.Pathfinding.Scripts;
using UnityEngine;

namespace Features.Passengers
{
    public class DemoPassengerSpawn : MonoBehaviour
    {
        [SerializeField] private GameObject _passenger;
        [SerializeField] private Vector3Int _spawnPoint;
        [SerializeField] private float _waitTime;

        private void Start()
        {
            
        }

        private IEnumerator SpawnPassenger()
        {
            while (true)
            {
                CreatePassenger();
                yield return new WaitForSeconds(_waitTime);
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Z))
                CreatePassenger();
            if (Input.GetKeyDown(KeyCode.X))
                StartCoroutine(SpawnPassenger());
        }

        private void CreatePassenger()
        {
            Instantiate(_passenger, _spawnPoint, Quaternion.identity);
        }
    }
}