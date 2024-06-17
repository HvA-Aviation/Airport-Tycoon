using Features.Managers;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;

public class DayCycle : MonoBehaviour
{
    [SerializeField, Tooltip("How many days need to be passed before a new period starts")] private int _daysInAPeriod;
    [SerializeField, Tooltip("How many seconds need to be passed before a new day starts")] private int _secondsInADay;
    [SerializeField, Tooltip("How many periods need to be passed before a new year starts")] private int _periodsInAYear;

    [SerializeField, Tooltip("A list with all the periodnames")] private List<string> _periodNames;

    public float SecondsPassed { get; private set; }
    public int DaysPassed { get; private set; }    
    public int PeriodsPassed{  get; private set; }
    public int YearsPassed {  get; private set; }

    private void FixedUpdate()
    {
        SecondsPassed += (1 * GameManager.Instance.GameTimeManager.DeltaTime);

        if (SecondsPassed >= _secondsInADay)
        {
            ProceedToNextDay();
        }
    }

    /// <summary>
    /// Call this function to proceed to the next day
    /// </summary>
    public void ProceedToNextDay()
    {
        DaysPassed++;
        SecondsPassed = 0;
        if(DaysPassed >= _daysInAPeriod)
        {
            ProceedToNextPeriod();
        }
    }

    /// <summary>
    /// Call This Function to Proceed to the next period
    /// </summary>
    public void ProceedToNextPeriod()
    {
        PeriodsPassed++;
        GameManager.Instance.FinanceManager.Balance.Subtract(GameManager.Instance.StaffManager.GetSalaryOwed());
        if(PeriodsPassed >= _periodsInAYear)
        {
            ProceedToNextYear();
        }
        DaysPassed = 0;
    }

    /// <summary>
    /// Call this function to proceed to the next year
    /// </summary>
    public void ProceedToNextYear()
    {
        YearsPassed++;
        PeriodsPassed = 0;
    }

    /// <summary>
    /// Call this function when you want to get the period name it is currently in
    /// </summary>
    /// <returns>The name of the period</returns>
    public string GetPeriodName()
    {
        return _periodNames[PeriodsPassed];
    }

    /// <summary>
    /// Call this function to get the total amount of days that has passed
    /// </summary>
    /// <returns>The total amount of days that has passed</returns>
    public int TotalDaysPassed()
    {
        return PeriodsPassed * _daysInAPeriod + DaysPassed;
    }

    /// <summary>
    /// Call this function to get the totak anount of periods that has passed
    /// </summary>
    /// <returns>The total amount of periods that has passed</returns>
    public int TotalPeriodsPassed()
    {
        return YearsPassed * _periodsInAYear + PeriodsPassed;
    }
}
