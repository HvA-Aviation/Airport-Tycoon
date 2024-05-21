using UnityEngine;

public class Employee : MonoBehaviour
{
    [SerializeField] private StaffManager _staffManager;

    [SerializeField] private int _hireCost;
    [SerializeField] private int _loanAmount;

    [SerializeField] Job _job;

    public int HireCost => _hireCost;
    public int LoanAmount => _loanAmount;
    public Job JobSite => _job;

    public enum Job { all }
}
