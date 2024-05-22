using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class SelectStaffTypeDemo : MonoBehaviour
{
    [SerializeField] private StaffManager _staffManager;
    [SerializeField] private TMP_Dropdown _hireDropDown;
    [SerializeField] private TMP_Dropdown _fireDropDown;

    private EmployeeTypes.EmployeeType _employeeTypeToHire;

    private string[] _employeeTypes;

    private void Awake()
    {
        _employeeTypes = Enum.GetNames(typeof(EmployeeTypes.EmployeeType));
    }

    private void Start()
    {
        _fireDropDown.ClearOptions();

        _fireDropDown.AddOptions(_employeeTypes.ToList());
    }

    public void SetEmployeeToHire(int val)
    {
        _employeeTypeToHire = (EmployeeTypes.EmployeeType)val;
    }

    public void Hire()
    {
        _staffManager.HireEmployee(_employeeTypeToHire);
    }

    public void SetEmployeeToFire(int val)
    {
        _staffManager.FireEmployee(val);
    }
}
