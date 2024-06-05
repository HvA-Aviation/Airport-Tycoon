using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MonthlyTime : MonoBehaviour
{
    [SerializeField] private int _timeUntilMonthItDone;

    private float timer = 0;

    public UnityEvent MonthDoneEvent = new UnityEvent();

    private void Update()
    {
        
    }
}
