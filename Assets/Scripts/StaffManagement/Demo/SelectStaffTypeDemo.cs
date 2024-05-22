using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class SelectStaffTypeDemo : MonoBehaviour
{
    [SerializeField] private StaffManager _staffManager;
    [SerializeField] private TMP_Dropdown _dropDown;

    private EmployeeTypes.EmployeeType _employeeTypeToHire;

    private string[] _employeeTypes;

    private void Awake()
    {
        _employeeTypes = Enum.GetNames(typeof(EmployeeTypes.EmployeeType));
    }

    private void Start()
    {
        _dropDown.ClearOptions();

        _dropDown.AddOptions(_employeeTypes.ToList());
    }

    public void SetEmployeeToHire(int val)
    {
        _employeeTypeToHire = (EmployeeTypes.EmployeeType)val;
        Debug.Log(_employeeTypeToHire);
    }

    public void Hire()
    {
        _staffManager.HireEmployees(_employeeTypeToHire);
    }
}
