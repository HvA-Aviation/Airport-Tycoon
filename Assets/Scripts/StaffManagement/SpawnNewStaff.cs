using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnNewStaff : MonoBehaviour
{
    [SerializeField] private GameObject _employeeOBJ;

    public GameObject NewEmployeeObjectSpawned {  get; private set; }

    public void InstantiateEmployee(EmployeeTypes.EmployeeType type)
    {
        GameObject newEmployeeOBJ = Instantiate(_employeeOBJ, Vector3.zero, Quaternion.identity);
        NewEmployeeObjectSpawned = newEmployeeOBJ;
    }
}
