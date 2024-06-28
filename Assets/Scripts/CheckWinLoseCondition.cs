using Features.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckWinLoseCondition : MonoBehaviour
{
    [SerializeField, Tooltip("How much money someone needs to win the game")]
    private int _winAmount;
    [SerializeField, Tooltip("How far in the red someone needs to be to lose the game")]
    private int _loseAmount;

    [SerializeField] private int _sceneIndex;

    private void Start()
    {
        GameManager.Instance.EventManager.Subscribe(Features.EventManager.EventId.OnMoneyAdded, (args) => CheckWinCondition());
        GameManager.Instance.EventManager.Subscribe(Features.EventManager.EventId.OnMoneyRemoved, (args) => CheckLoseCondition());
    }

    public void CheckWinCondition()
    {
        if (GameManager.Instance.FinanceManager.Balance.Value >= _winAmount)
            SceneManager.LoadScene(_sceneIndex);
    }

    public void CheckLoseCondition()
    {
        if (GameManager.Instance.FinanceManager.Balance.Value <= _loseAmount)
            SceneManager.LoadScene(_sceneIndex);
    }
}
