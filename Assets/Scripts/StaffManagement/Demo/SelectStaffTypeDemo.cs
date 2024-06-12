using System;
using System.Linq;
using Features.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class SelectStaffTypeDemo : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown _hireDropDown;

    [SerializeField] private Transform _parent;
    [SerializeField] private GameObject _listObject;

    private int _nHiredEmployees = 0;
    private Employee.EmployeeTypes _employeeTypeToHire;

    private string[] _employeeTypes;

    public UnityEvent Event = new UnityEvent();

    private void Awake()
    {
        _employeeTypes = Enum.GetNames(typeof(Employee.EmployeeTypes));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            transform.GetChild(0).gameObject.SetActive(!transform.GetChild(0).gameObject.activeSelf);
        }
    }

    private void Start()
    {
        _hireDropDown.ClearOptions();

        _hireDropDown.AddOptions(_employeeTypes.ToList());
    }

    public void SetEmployeeToHire(int val)
    {
        _employeeTypeToHire = (Employee.EmployeeTypes)val;
    }

    public void Hire()
    {
        GameManager.Instance.StaffManager.HireEmployee(_employeeTypeToHire);
        if(_nHiredEmployees == GameManager.Instance.StaffManager.Employees.Count) return;

        _nHiredEmployees++;
        string name = GameManager.Instance.StaffManager.GetEmployeeName(GameManager.Instance.StaffManager.LastEmployeeID);

        GameObject temp = Instantiate(_listObject, _parent);
        temp.GetComponent<ShowStaffInList>().ThisEmployee = GameManager.Instance.StaffManager.Employees[GameManager.Instance.StaffManager.LastEmployeeID];
        temp.GetComponent<ShowStaffInList>().Demo = this;
    }

    public void Fire()
    {
        _nHiredEmployees--;
        Event.Invoke();
        Event.RemoveAllListeners();
    }
}
