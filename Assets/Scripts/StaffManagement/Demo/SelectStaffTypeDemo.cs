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

    private int _fireEmployeeID;

    private EmployeeTypes.EmployeeType _employeeTypeToHire;

    private string[] _employeeTypes;
    private List<string> _employees = new List<string>();
    private void Awake()
    {
        _employeeTypes = Enum.GetNames(typeof(EmployeeTypes.EmployeeType));
    }

    private void Start()
    {
        _hireDropDown.ClearOptions();

        _hireDropDown.AddOptions(_employeeTypes.ToList());

        _fireDropDown.ClearOptions();
    }

    public void SetEmployeeToHire(int val)
    {
        _employeeTypeToHire = (EmployeeTypes.EmployeeType)val;
    }

    public void Hire()
    {
        _staffManager.HireEmployee(_employeeTypeToHire);
        string name = _staffManager.GetNameOfEmployee(_staffManager.LastEmployeeID);
        Debug.Log(_staffManager.LastEmployeeID);
        _employees.Add(name);
        UpdateFireDropDown();
    }

    public void SetEmployeeToFire(int val)
    {
        _fireEmployeeID = val;
        
    }

    public void Fire()
    {
        _employees.Remove(_staffManager.GetNameOfEmployee(_fireEmployeeID));
        _staffManager.FireEmployee(_fireEmployeeID);
        UpdateFireDropDown();
    }

    void UpdateFireDropDown()
    {
        _fireDropDown.ClearOptions();
        _fireDropDown.AddOptions(_employees);
    }
}
