using UnityEngine;

public class Employee : MonoBehaviour
{
    [SerializeField] private int _minimumSalary, _maximumSalary;
    public string Name { get; private set; }
    public int SalaryAmount { get; private set; }
    public int EmployeeID { get; private set; }
    public EmployeeTypes EmployeeType { get; private set; }

    private void Awake()
    {
        Name = StaffNames.GetRandomFirstName() + " " + StaffNames.GetRandomLastName();
        SalaryAmount = Random.Range(_minimumSalary, _maximumSalary);
    }

    public void SetEmployeeType(EmployeeTypes type) => EmployeeType = type;

    public void SetSalaryAmount(int amount) => SalaryAmount = amount;

    public void SetID(int id) => EmployeeID = id;

    public enum EmployeeTypes
    {
        Builder = 0,
        Security = 1,
        Staff = 2,
    }
}
