using Features.Managers;
using UnityEngine;

public class FinanceDemo : MonoBehaviour
{
    [SerializeField]
    private UIDemoManager _demoManager;

    private void Start()
    {
        //Set Starting money
        GameManager.Instance.FinanceManager.Balance.Add(100);
        
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
            GameManager.Instance.FinanceManager.AdvancePeriod();
            UpdateUI();
        }
    }

    public void TryAcceptLoan(int index)
    {
        if (GameManager.Instance.FinanceManager.GetLoan(index).State == LoanState.Idle)
        {
            GameManager.Instance.FinanceManager.AcceptLoan(index);
            Debug.Log($"Loan {index} is accepted.");
        }
        else
        {
            Debug.Log($"Loan {index} is already in progress.");
        }
    }

    public void UpdateUI()
    {
        for(int i = 0; i < 3; i++)
        {
            _demoManager.UpdateLoanCard(i, GameManager.Instance.FinanceManager.GetLoan(i));
        }
    }
}
