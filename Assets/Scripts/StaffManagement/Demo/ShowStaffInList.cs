using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowStaffInList : MonoBehaviour
{
    [SerializeField] private TMP_Text _nameText, _jobTypeText, _salaryText;

    [HideInInspector]public Employee ThisEmployee;

    private void Start()
    {
        _nameText.text = ThisEmployee.Name;
        _jobTypeText.text = ThisEmployee.EmployeeType.ToString();
        _salaryText.text = ThisEmployee.LoanAmount.ToString();
    }
}
