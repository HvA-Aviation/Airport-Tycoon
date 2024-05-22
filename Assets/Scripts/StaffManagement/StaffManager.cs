using System.Collections.Generic;
using UnityEngine;

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

    public void HireEmployees(EmployeeTypes.EmployeeType type)
    {
        _spawnNewStaff.InstantiateEmployee(type);

        Employee employee = _spawnNewStaff.NewEmployeeObjectSpawned.GetComponent<Employee>();

        employee.SetEmployeeType(type);

        AddEmployeeToDictionary(employee);
    }
}
