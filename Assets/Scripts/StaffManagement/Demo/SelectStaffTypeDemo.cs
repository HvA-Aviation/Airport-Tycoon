using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class SelectStaffTypeDemo : MonoBehaviour
{    
    [SerializeField] private TMP_Dropdown _hireDropDown;

    [SerializeField] private Transform _parent;
    [SerializeField] private GameObject _listObject;

    private EmployeeTypes.EmployeeType _employeeTypeToHire;

    private string[] _employeeTypes;

    public StaffManager StaffManager;
    public UnityEvent Event = new UnityEvent();

    private void Awake()
    {
        _employeeTypes = Enum.GetNames(typeof(EmployeeTypes.EmployeeType));
    }

    private void Start()
    {
        _hireDropDown.ClearOptions();

        _hireDropDown.AddOptions(_employeeTypes.ToList());
    }

    public void SetEmployeeToHire(int val)
    {
        _employeeTypeToHire = (EmployeeTypes.EmployeeType)val;
    }

    public void Hire()
    {
        StaffManager.HireEmployee(_employeeTypeToHire);
        string name = StaffManager.GetNameOfEmployee(StaffManager.LastEmployeeID);
        Debug.Log(StaffManager.LastEmployeeID);

        GameObject temp = Instantiate(_listObject, _parent);
        temp.GetComponent<ShowStaffInList>().ThisEmployee = StaffManager.Employees[StaffManager.LastEmployeeID];
        temp.GetComponent<ShowStaffInList>().Demo = this;
    }    

    public void Fire()
    {
        Event.Invoke();
        Event.RemoveAllListeners();
    }    
}
