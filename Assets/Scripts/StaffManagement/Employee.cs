using UnityEngine;

public class Employee : MonoBehaviour
{
    [SerializeField] private StaffManager _staffManager;
    public int LoanAmount { get; private set; }
    public EmployeeTypes.EmployeeType EmployeeType {  get; private set; }

    public void SetEmployeeType(EmployeeTypes.EmployeeType type) => EmployeeType = type;

    public void SetLoanAmount(int amount) => LoanAmount = amount;   
}
