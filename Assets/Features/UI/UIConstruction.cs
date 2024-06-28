using System;
using UnityEngine;

namespace Features.UI
{
    public class UIConstruction : MonoBehaviour
    {
        [SerializeField] private GameObject _cursor;

        private void OnEnable()
        {
            _cursor.SetActive(true);
        }

        private void OnDisable()
        {
            _cursor.SetActive(false);
        }
    }
}