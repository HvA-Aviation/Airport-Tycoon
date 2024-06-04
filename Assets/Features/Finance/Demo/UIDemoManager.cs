using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class UIDemoManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _moneyText;

    [SerializeField]
    private List<LoanCard> _loanCards;

    public void SetMoney(int value)
    {
        _moneyText.text = "Money: " + value.ToString();
    }

    public void UpdateLoanCard(int index, Loan<LoanSO> loan)
    {
        _loanCards[index].UpdateCard(loan);
    }
}
