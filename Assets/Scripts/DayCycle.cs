using Features.Managers;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public struct MonthAndDays
{
    public string monthName;
    public int daysInMonth;
}

public class DayCycle : MonoBehaviour
{
    [SerializeField, Tooltip("How many seconds need to be passed before a new day starts")] private int _secondsInADay;
    [SerializeField, Tooltip("How many days need to be passed before a new period starts")] private int _daysInAMonth;
    [SerializeField, Tooltip("How many periods need to be passed before a new year starts")] private int _periodsInAYear;

    [SerializeField, Tooltip("A list with all the periodnames")] private List<MonthAndDays> _periodNames;

    public float SecondsPassed { get; private set; }
    public int DaysPassed { get; private set; }
    public int PeriodsPassed { get; private set; }
    public int YearsPassed { get; private set; }

    public string GetDayMonthDay => $"{GetMonthName()} {DaysPassed}, Year {YearsPassed}";

    public UnityEvent OnPeriodPassed = new UnityEvent();

    void Start()
    {
        DaysPassed = 1;
    }

    private void FixedUpdate()
    {
        SecondsPassed += GameManager.Instance.GameTimeManager.DeltaTime;

        if (SecondsPassed >= _secondsInADay)
        {
            GameManager.Instance.FinanceManager.AdvancePeriod();
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
        if (DaysPassed > GetDaysInCurrentMonth())
        {
            ProceedToNextMonth();
        }
    }

    /// <summary>
    /// Call This Function to Proceed to the next period
    /// </summary>
    public void ProceedToNextMonth()
    {
        PeriodsPassed++;
        OnPeriodPassed?.Invoke();

        if (PeriodsPassed >= _periodsInAYear)
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
    public string GetMonthName()
    {
        return _periodNames[PeriodsPassed].monthName;
    }

    /// <summary>
    /// Returns the number of days in the current month.
    /// </summary>
    /// <returns>The number of days in the current month.</returns>
    public int GetDaysInCurrentMonth()
    {
        return _periodNames[PeriodsPassed].daysInMonth;
    }

    /// <summary>
    /// Call this function to get the total amount of days that has passed
    /// </summary>
    /// <returns>The total amount of days that has passed</returns>
    public int TotalDaysPassed()
    {
        return PeriodsPassed * _daysInAMonth + DaysPassed;
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
