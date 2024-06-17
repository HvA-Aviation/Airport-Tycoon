using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmployeeDemo : MonoBehaviour
{
    private Employee _employee;
    private void OnEnable()
    {
        if(_employee == null)
            _employee = GetComponent<Employee>();

        _employee.SetEmployeeSettings();
    }
}
