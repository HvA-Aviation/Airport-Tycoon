using UnityEngine;

public class FinanceDemo : MonoBehaviour
{
    [SerializeField]
    private UIDemoManager _demoManager;

    [SerializeField]
    private FinanceManager _manager;

    private void Start()
    {
        UpdateUI();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            TryAcceptLoan(0);
            UpdateUI();
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            TryAcceptLoan(1);
            UpdateUI();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            TryAcceptLoan(2);
            UpdateUI();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            _manager.AdvancePeriod();
            UpdateUI();
        }
    }

    private void TryAcceptLoan(int index)
    {
        if (_manager.GetLoan(index).State == LoanState.Idle)
        {
            _manager.AcceptLoan(index);
            Debug.Log($"Loan {index} is accepted.");
        }
        else
        {
            Debug.Log($"Loan {index} is already in progress.");
        }
    }

    private void UpdateUI()
    {
        _demoManager.SetMoney(_manager.Balance.Value);
        for(int i = 0; i < 3; i++)
        {
            _demoManager.UpdateLoanCard(i, _manager.GetLoan(i));
        }
    }
}
