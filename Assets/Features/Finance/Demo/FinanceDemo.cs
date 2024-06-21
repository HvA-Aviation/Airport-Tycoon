using Features.Managers;
using UnityEngine;

public class FinanceDemo : MonoBehaviour
{
    [SerializeField]
    private UIDemoManager _demoManager;

    private void Start()
    {
        UpdateUI();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Alpha1) && Input.GetKeyDown(KeyCode.Alpha2))
        {
            GameManager.Instance.FinanceManager.Balance.Add(100);
        }

        if (Input.GetKey(KeyCode.Alpha1) && Input.GetKeyDown(KeyCode.Alpha3))
        {
            GameManager.Instance.FinanceManager.Balance.Subtract(100);
        }

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
    }

    public void TryAcceptLoan(int index)
    {
        if (GameManager.Instance.FinanceManager.GetLoan(index).State == LoanState.Idle)
        {
            GameManager.Instance.FinanceManager.AcceptLoan(index);
        }
        else
        {
            Debug.Log($"Loan {index} is already in progress.");
        }
        
        UpdateUI();
    }

    private void UpdateUI()
    {
        for (int i = 0; i < 3; i++)
        {
            _demoManager.UpdateLoanCard(i, GameManager.Instance.FinanceManager.GetLoan(i));
        }
    }
}
