using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class MenuInteractables : MonoBehaviour, IInteractable
{
    [SerializeField] private UIManager _uiManager;
    private CanvasGroup _canvasGroup;

    private void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();

        _uiManager.Interactables.Add(this);
    }

    public void DissableInteraction() => _canvasGroup.interactable = false;
    
    public void EnableInteraction() => _canvasGroup.interactable = true;    
}
