using System;
using Features.EventManager;
using Features.Managers;
using TMPro;
using UnityEngine;

namespace Features.Finance
{
    public class BalanceDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _textMesh;
        
        private void Start()
        {
            GameManager.Instance.EventManager.Subscribe(EventId.OnMoneyAdded, (args) => UpdateBalance());
            GameManager.Instance.EventManager.Subscribe(EventId.OnMoneyRemoved, (args) => UpdateBalance());
            UpdateBalance();
        }

        private void UpdateBalance()
        {
            _textMesh.text = GameManager.Instance.FinanceManager.Balance.Value.ToString();
        }
    }
}