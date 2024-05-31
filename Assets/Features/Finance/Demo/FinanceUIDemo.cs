using System;
using Features.EventManager;
using Features.Managers;
using TMPro;
using UnityEngine;

namespace Features.Finance.Demo
{
    public class FinanceUIDemo : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _textMeshPro;
        
        private void Start()
        {
            GameManager.Instance.EventManager.Subscribe(EventId.OnMoneyAdded, (args) => UpdateUI());
            GameManager.Instance.EventManager.Subscribe(EventId.OnMoneyRemoved, (args) => UpdateUI());

            //Set Starting money
            GameManager.Instance.FinanceManager.Balance.Add(100);
            
            UpdateUI();
        }

        private void UpdateUI()
        {
            _textMeshPro.text = "€" + GameManager.Instance.FinanceManager.Balance.Value;
        }
    }
}