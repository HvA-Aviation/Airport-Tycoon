using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class UIDemoManager : MonoBehaviour
{
    [SerializeField]
    private List<LoanCard> _loanCards;

    public void UpdateLoanCard(int index, Loan<LoanSO> loan)
    {
        _loanCards[index].UpdateCard(loan);
    }
}
