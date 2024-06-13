using System;
using System.Collections;
using System.Collections.Generic;
using Features.RandomPassenger.Scripts;
using UnityEngine;
using Random = UnityEngine.Random;

public class PassengerCreator : MonoBehaviour
{
    [SerializeField] private List<Sprite> _hair;
    [SerializeField] private List<Sprite> _clothes;
    [SerializeField] private List<Sprite> _misc;
    [SerializeField] private List<Sprite> _body;

    [SerializeField] private GameObject _base;


    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Alpha0))
        {
            foreach (var passengerVisual in FindObjectsByType<PassengerVisual>(FindObjectsSortMode.None))
            {
                Destroy(passengerVisual.gameObject);
            }
            
            GameObject instance = Instantiate(_base.gameObject, new Vector3(6.5f, 4), Quaternion.identity);
            instance.GetComponent<PassengerVisual>().SetVisuals(_hair[Random.Range(0, _hair.Count)],
                _clothes[Random.Range(0, _clothes.Count)], _misc[Random.Range(0, _misc.Count)],
                _body[Random.Range(0, _body.Count)]);
        }
    }
}