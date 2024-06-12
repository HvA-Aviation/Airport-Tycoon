using Features.Managers;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;

public class MonthlyTime : MonoBehaviour
{
    [SerializeField] private int _daysInAMonth;
    [SerializeField] private int _secondsInADay;

    private float _secondsPassed;
    private int _daysPassed;
    private int _monthsPassed;

    public static UnityEvent PaySalaray = new UnityEvent();
    private void FixedUpdate()
    {
        _secondsPassed += (1 * GameManager.Instance.GameTimeManager.DeltaTime);

        if (_secondsPassed >= _secondsInADay)
        {
            _daysPassed++;
            _secondsPassed = 0;
        }

        if(_daysPassed >= _daysInAMonth)
        {
            _monthsPassed++;
            PaySalaray.Invoke();            
            _daysPassed = 0;
        }
    }
}
