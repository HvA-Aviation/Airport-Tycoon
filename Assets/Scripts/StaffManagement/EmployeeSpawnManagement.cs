using UnityEngine;

public class EmployeeSpawnManagement : MonoBehaviour
{
    [SerializeField] private GameObject _employeeOBJ;

    private GameObject _parentOBJ;

    private void Start() => _parentOBJ = new GameObject("EmployeesParent");

   /// <summary>
   /// This function will return an employee that is instantiated of the type of employee that is spawned
   /// </summary>
   /// <param name="type">The type of employee that needs to be spawned</param>
   /// <returns>The Employee of the type of employee</returns>
    public Employee InstantiateEmployee(Employee.EmployeeTypes type)
    {
        return Instantiate(_employeeOBJ, Vector3.zero, Quaternion.identity, _parentOBJ.transform).GetComponent<Employee>();        
    }

    /// <summary>
    /// This function is called when you fire an employee and then destroy the object
    /// </summary>
    /// <param name="employeeOBJ">The object that needs to be destroyed</param>
    public void DespawnEmployeeOBJ(GameObject employeeOBJ)
    {
        Destroy(employeeOBJ);
    }
}
