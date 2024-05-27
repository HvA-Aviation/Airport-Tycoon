using UnityEngine;

public class Employee : MonoBehaviour
{
    [SerializeField] private StaffManager _staffManager;
    [SerializeField] private StaffNames _staffNames;
    public string Name { get; private set; }
    public int SalaryAmount { get; private set; }
    public int EmployeeID { get; private set; }
    public EmployeeTypes.EmployeeType EmployeeType { get; private set; }

    private void Awake()
    {
        Name = _staffNames.GetRandomFirstName() + " " + _staffNames.GetRandomLastName();
    }

    public void SetEmployeeType(EmployeeTypes.EmployeeType type) => EmployeeType = type;

    public void SetSalaryAmount(int amount) => SalaryAmount = amount;

    public void SetID(int id) => EmployeeID = id;
}
