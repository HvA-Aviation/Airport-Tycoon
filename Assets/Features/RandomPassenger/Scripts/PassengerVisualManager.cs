using System;
using System.Collections;
using System.Collections.Generic;
using Features.RandomPassenger.Scripts;
using UnityEngine;
using Random = UnityEngine.Random;

public class PassengerVisualManager : MonoBehaviour
{
    [SerializeField] private List<Sprite> _hair;
    [SerializeField] private List<Sprite> _clothes;
    [SerializeField] private List<Sprite> _misc;
    [SerializeField] private List<Sprite> _body;

    /// <summary>
    /// Takes a passenger visual and assigns a random sprite to i
    /// </summary>
    /// <param name="passengerVisual">A passenger with the sprites seperated</param>
    public void UpdateSkin(PassengerVisual passengerVisual)
    {
        passengerVisual.SetVisuals(_hair[Random.Range(0, _hair.Count)],
            _clothes[Random.Range(0, _clothes.Count)], _misc[Random.Range(0, _misc.Count)],
            _body[Random.Range(0, _body.Count)]);
    }
}