using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShowStaffInList : MonoBehaviour
{
    [SerializeField] private TMP_Text _nameText, _jobTypeText, _salaryText;

    private Button _button;

    [HideInInspector]public Employee ThisEmployee;
    [HideInInspector] public SelectStaffTypeDemo Demo;

    private void Start()
    {
        _button = GetComponent<Button>();
        _nameText.text = ThisEmployee.Name;
        _jobTypeText.text = ThisEmployee.EmployeeType.ToString();
        _salaryText.text = ThisEmployee.LoanAmount.ToString();
        _button.onClick.AddListener(Select);
    }

    private void Select()
    {
        Demo.Event.RemoveAllListeners();
        Demo.Event.AddListener(Remove);
    }

    private void Remove()
    {
        Demo.StaffManager.FireEmployee(ThisEmployee.EmployeeID);
        Destroy(gameObject);
    }


}
