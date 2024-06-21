using System;
using UnityEngine;

public class Employee : MonoBehaviour
{
    private Action<Employee> _returnCallback;

    public string Name { get; private set; }
    public int SalaryAmount { get; private set; }
    public int EmployeeID { get; private set; }
    public EmployeeTypes EmployeeType { get; private set; }

    private void Awake()
    {
        Name = StaffNames.GetRandomFirstName() + " " + StaffNames.GetRandomLastName();
    }

    public void SetEmployeeType(EmployeeTypes type) => EmployeeType = type;

    public void SetSalaryAmount(int amount) => SalaryAmount = amount;

    public void SetID(int id) => EmployeeID = id;

    public void SetCallback(Action<Employee> callback) => _returnCallback = callback;

    public void Return() => _returnCallback?.Invoke(this);

    public enum EmployeeTypes
    {
        Builder = 0,
        Security = 1,
        Staff = 2,
    }
}
