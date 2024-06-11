using System.Collections.Generic;
using UnityEngine;

public class StaffManager : MonoBehaviour
{
    [SerializeField] private EmployeeSpawnManagement _spawnNewStaff;

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
        Employee employee = _spawnNewStaff.InstantiateEmployee(type);
        
        employee.SetEmployeeType(type);

        employee.SetID(_nextEmployeeID);

        AddEmployeeToDictionary(employee);
    }

    /// <summary>
    /// This function is called when you want to fire an employee
    /// </summary>
    /// <param name="employeeID">The index of the employee you want to fire</param>
    public void FireEmployee(int employeeID)
    {
        if (Employees.TryGetValue(employeeID, out Employee employee))
        {
            _spawnNewStaff.DespawnEmployeeOBJ(employee.gameObject);
            Employees.Remove(employeeID);
        }
    }

    public GameObject GetEmployeeGameObject(int employeeID) => Employees[employeeID].gameObject;

    public int GetEmployeeSalary(int employeeID) => Employees[employeeID].SalaryAmount;

    public string GetEmployeeName(int employeeID) => Employees[employeeID].Name;

    public Employee.EmployeeTypes GetEmployeeType(int employeeID) => Employees[employeeID].EmployeeType;
}
