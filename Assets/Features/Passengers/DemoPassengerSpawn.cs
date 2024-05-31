using System;
using System.Collections;
using Implementation.Pathfinding.Scripts;
using UnityEngine;

namespace Features.Passengers
{
    public class DemoPassengerSpawn : MonoBehaviour
    {
        [SerializeField] private GameObject _passenger;
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
            if (Input.GetKeyDown(KeyCode.H))
                CreatePassenger();
            if (Input.GetKeyDown(KeyCode.K))
                StartCoroutine(SpawnPassenger());
        }

        private void CreatePassenger()
        {
            Instantiate(_passenger, new Vector3(1, 0, 0), Quaternion.identity);
        }
    }
}