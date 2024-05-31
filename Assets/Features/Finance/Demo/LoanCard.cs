using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LoanCard : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _bankTitle;

    [SerializeField]
    private TextMeshProUGUI _amount;

    [SerializeField]
    private TextMeshProUGUI _interest;

    [SerializeField]
    private TextMeshProUGUI _period;

    [SerializeField]
    private TextMeshProUGUI _status;

    [SerializeField]
    private FinanceDemo _financeDemo;

    public void UpdateCard(Loan<LoanSO> loan)
    {
        _bankTitle.text = loan.Information.BankTitle;
        _amount.text = $"Amount: {loan.Information.Amount}";
        _interest.text = $"Interest: {loan.Information.Interest}%";
        _period.text = $"Period: {loan.Information.PaymentPeriod} periods";
        _status.text = $"Status: {loan.State}";
    }
}
