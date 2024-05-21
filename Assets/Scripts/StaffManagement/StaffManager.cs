using System.Collections.Generic;
using UnityEngine;

public class StaffManager : MonoBehaviour
{
    public List<Employee> Employees { get; private set; }

    private void Awake()
    {
        Employees = new List<Employee>();
    }

    public void HireEmployees()
    {
        Employees.Add(new Employee());
    }
}
