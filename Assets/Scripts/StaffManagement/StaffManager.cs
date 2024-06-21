using Features.Managers;
using System;
using System.Collections.Generic;
using Features.Workers;
using UnityEngine;

public class StaffManager : MonoBehaviour
{
    [SerializeField] private List<EmployeePool> _employeePools;
    [SerializeField, Tooltip("The cost to hire new staff")]
    private int _recruitmentCost;

    private int _nextEmployeeID = 0;
    public int LastEmployeeID => _nextEmployeeID - 1;
    public Dictionary<int, Employee> Employees { get; private set; }

    private void Awake() => Employees = new Dictionary<int, Employee>();

    /// <summary>
    /// Adds an employee to the dictionary with the employeesID connected to it
    /// </summary>
    /// <param name="employee">The employee that needs to be added to the dictionary</param>
    private void AddEmployeeToDictionary(Employee employee)
    {
        Employees.Add(_nextEmployeeID, employee);

        _nextEmployeeID++;
    }

    /// <summary>
    /// This function is called when you want to hire a type of employee
    /// </summary>
    /// <param name="type">The type of employee you want to hire</param>
    public void HireEmployee(Employee.EmployeeTypes type)
    {
        if (GameManager.Instance.FinanceManager.Balance.Value < _recruitmentCost)
        {
            return;
        }

        foreach (var pool in _employeePools)
        {
            if (type != pool.PoolType)
                continue;

            Employee employee = pool.Pool.EmployeeObjectPool.Get();

            employee.SetID(_nextEmployeeID);

            AddEmployeeToDictionary(employee);

            break;
        }
    }

    /// <summary>
    /// This function is called when you want to fire an employee
    /// </summary>
    /// <param name="employeeID">The index of the employee you want to fire</param>
    public void FireEmployee(int employeeID)
    {
        if (Employees.TryGetValue(employeeID, out Employee employee))
        {
            employee.Return();
            Employees.Remove(employeeID);
            
            //TODO fix this monstrosity. I'm sorry I don't have enough time :(
            employee.GetComponent<AssignableWorker>()?.Fire();
            employee.GetComponent<BuilderWorker>()?.Fire();
        }
    }

    /// <summary>
    /// Call this function to pay the salary
    /// </summary>
    public void PayAllSalary() => GameManager.Instance.FinanceManager.Balance.Subtract(GetSalaryOwed());

    /// <summary>
    /// Get all the total owed salary from all the employees
    /// </summary>
    /// <returns>The total owed salary</returns>
    public int GetSalaryOwed()
    {
        int totalSalary = 0;
        for (int i = 0; i < Employees.Count; i++)
            totalSalary += Employees[i].SalaryAmount;

        return totalSalary;
    }

    public GameObject GetEmployeeGameObject(int employeeID) => Employees[employeeID].gameObject;

    public int GetEmployeeSalary(int employeeID) => Employees[employeeID].SalaryAmount;

    public string GetEmployeeName(int employeeID) => Employees[employeeID].Name;

    public Employee.EmployeeTypes GetEmployeeType(int employeeID) => Employees[employeeID].EmployeeType;

    [Serializable]
    public struct EmployeePool
    {
        public Employee.EmployeeTypes PoolType;
        public StaffObjectPool Pool;
    }
}
