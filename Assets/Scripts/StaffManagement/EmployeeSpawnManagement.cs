using System;
using System.Collections.Generic;
using UnityEngine;

public class EmployeeSpawnManagement : MonoBehaviour
{
    [SerializeField] private List<EmployeeObjects> _employees = new List<EmployeeObjects>();

    private Dictionary<Employee.EmployeeTypes, GameObject> _employeeObjects = new Dictionary<Employee.EmployeeTypes, GameObject>();

    private GameObject _parentOBJ;
    
    [SerializeField] private Vector3Int _spawnPosition;

    private void Awake()
    {
        foreach (EmployeeObjects employeeOBJ in _employees)
            _employeeObjects.Add(employeeOBJ.Type, employeeOBJ.Object);
    }

    private void Start() => _parentOBJ = new GameObject("EmployeesParent");

    /// <summary>
    /// This function will return an employee that is instantiated of the type of employee that is spawned
    /// </summary>
    /// <param name="type">The type of employee that needs to be spawned</param>
    /// <returns>The Employee of the type of employee</returns>
    public Employee InstantiateEmployee(Employee.EmployeeTypes type)
    {
        return Instantiate(_employeeObjects[type], _spawnPosition, Quaternion.identity, _parentOBJ.transform).GetComponent<Employee>();
    }

    /// <summary>
    /// This function is called when you fire an employee and then destroy the object
    /// </summary>
    /// <param name="employeeOBJ">The object that needs to be destroyed</param>
    public void DespawnEmployeeOBJ(GameObject employeeOBJ)
    {
        Destroy(employeeOBJ);
    }

    [Serializable]
    public struct EmployeeObjects
    {
        public Employee.EmployeeTypes Type;
        public GameObject Object;
    }
}
