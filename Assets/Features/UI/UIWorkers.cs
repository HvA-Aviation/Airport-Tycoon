using System;
using Features.EventManager;
using Features.Managers;
using TMPro;
using UnityEngine;

namespace Features.UI
{
    public class UIWorkers : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _workersAmount;
        private void Start()
        {
            GameManager.Instance.EventManager.Subscribe(EventId.OnEmployeeHire, UpdateAmount);
            GameManager.Instance.EventManager.Subscribe(EventId.OnEmployeeFire, UpdateAmount);
        }

        private void UpdateAmount(EventArgs args)
        {
            _workersAmount.text = GameManager.Instance.StaffManager.GetActiveAmountOfWorkers().ToString();
        }
    }
}