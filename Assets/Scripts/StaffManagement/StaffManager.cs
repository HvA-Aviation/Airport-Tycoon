using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StaffManager : MonoBehaviour
{
    [SerializeField] private SpawnNewStaff _spawnNewStaff;

    private int _nextEmployeeID = 0;
    
    public Dictionary<int, Employee> Employees { get; private set; }

    private void Awake()
    {
        Employees = new Dictionary<int, Employee>();
    }

    private void AddEmployeeToDictionary(Employee employee)
    {
        Employees.Add(_nextEmployeeID, employee);

        _nextEmployeeID++;
    }

    /// <summary>
    /// This function is called when you want to hire a type of employee
    /// </summary>
    /// <param name="type">The type of employee you want to hire</param>
    public void HireEmployee(EmployeeTypes.EmployeeType type)
    {
        _spawnNewStaff.InstantiateEmployee(type);

        Employee employee = _spawnNewStaff.NewEmployeeObjectSpawned.GetComponent<Employee>();

        employee.SetEmployeeType(type);

        AddEmployeeToDictionary(employee);
    }

    /// <summary>
    /// This function is called when you want to fire an employee
    /// </summary>
    /// <param name="employeeID">The index of the employee you want to fire</param>
    public void FireEmployee(int employeeID)
    {
        if(Employees.TryGetValue(employeeID, out Employee employee))
        {
            _spawnNewStaff.DespawnEmployeeOBJ(employeeID);
            Employees.Remove(employeeID);            
        }
    }
}
