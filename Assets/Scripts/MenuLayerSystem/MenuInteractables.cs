using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class MenuInteractables : MonoBehaviour
{
    [SerializeField] private UIManager _uiManager;
    private CanvasGroup _canvasGroup;

    private void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();

        _uiManager.CanvasGroups.Add(_canvasGroup);
    }
}
