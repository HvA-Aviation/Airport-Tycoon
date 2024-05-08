using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnableUI : MonoBehaviour
{    
    [SerializeField] private GameObject _menuGFX;

    [HideInInspector]public UnityEvent PressEnableUIButton = new UnityEvent();

    private void Awake()
    {
        PressEnableUIButton.AddListener(ShowUI);
        PressEnableUIButton.AddListener(PutScreenOnTop);
        PressEnableUIButton.AddListener(DeactivateInteractables);
    }    

    /// <summary>
    /// When the activate button is pressed the UI will be shown
    /// </summary>
    private void ShowUI() => _menuGFX.SetActive(!_menuGFX.activeSelf);
    
    /// <summary>
    /// Set the childpos to the last of the children so it is ontop of everything
    /// </summary>
    private void PutScreenOnTop() => transform.SetAsLastSibling();
    
    /// <summary>
    /// When the ui is enabled make all the other menus not interactable
    /// </summary>
    private void DeactivateInteractables()
    {
        foreach (CanvasGroup group in UIManager.Instance.CanvasGroups)
            group.interactable = false;
                 
        if(TryGetComponent<CanvasGroup>(out CanvasGroup thisGroup))
            thisGroup.interactable= true;
    }
}
