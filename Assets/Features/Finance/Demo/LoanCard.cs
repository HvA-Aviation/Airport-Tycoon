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
    private Button _button;

    public void UpdateCard(Loan<LoanSO> loan)
    {
        _bankTitle.text = loan.Information.BankTitle;
        _amount.text = $"{loan.Information.Amount}";
        _interest.text = $"{loan.Information.Interest}%";
        _period.text = $"{loan.Information.PaymentPeriod} periods";
        _status.text = $"{loan.State}";
        _button.interactable = loan.State == LoanState.Idle;
    }
}
