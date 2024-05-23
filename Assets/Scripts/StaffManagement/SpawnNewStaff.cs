using UnityEngine;

public class SpawnNewStaff : MonoBehaviour
{
    [SerializeField] private GameObject _employeeOBJ;
    [SerializeField] private StaffManager _staffManager;

    private GameObject _parentOBJ;

    public GameObject NewEmployeeObjectSpawned { get; private set; }

    private void Start() => _parentOBJ = new GameObject("EmployeesParent");

    /// <summary>
    /// This function is called when you hired an employee and the employee is then seen on the screen
    /// </summary>
    /// <param name="type">The type of employee to instantiate</param>
    public void InstantiateEmployee(EmployeeTypes.EmployeeType type)
    {
        GameObject newEmployeeOBJ = Instantiate(_employeeOBJ, Vector3.zero, Quaternion.identity, _parentOBJ.transform);
        NewEmployeeObjectSpawned = newEmployeeOBJ;
    }

    /// <summary>
    /// This function is called when you fire an employee and then destroy the object
    /// </summary>
    /// <param name="employeeID">The employeeID you want to destroy</param>
    public void DespawnEmployeeOBJ(int employeeID)
    {
        Destroy(_staffManager.Employees[employeeID].gameObject);
    }
}
