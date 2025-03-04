using System.Collections.Generic;
using UnityEngine;

using LoanType = LoanSO;

public class FinanceManager : MonoBehaviour
{
    [Header("Loans")]
    [SerializeField]
    private List<LoanType> _listOfLoans;

    public LoanManager<LoanType> _loanManager;

    public Balance Balance { get; private set; } = new Balance();

    private void Awake()
    {
        _loanManager = new LoanManager<LoanType>(_listOfLoans);
    }

    public void AdvancePeriod()
    {
        Balance.Subtract((int)_loanManager.AdvancePeriod());
    }

    public void AcceptLoan(int index) 
    { 
        Balance.Add((int)_loanManager.AcceptLoan(index)); 
    }

    public Loan<LoanType> GetLoan(int index) => _loanManager.GetLoan(index);
}
