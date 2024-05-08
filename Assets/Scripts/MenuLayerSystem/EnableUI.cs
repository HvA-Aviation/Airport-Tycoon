using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnableUI : MonoBehaviour
{
    [SerializeField] private KeyCode _dissableEnableScreen;
    [SerializeField] private GameObject _menuGFX;
    [SerializeField] private CanvasGroup _canvasGroup;

    public UnityEvent PressEnableUIButton = new UnityEvent();

    private void Awake()
    {
        PressEnableUIButton.AddListener(ActivateUI);
        PressEnableUIButton.AddListener(SetChildPos);
        PressEnableUIButton.AddListener(DeactivateInteractables);
    }

    private void Start()
    {
        UIManager.Instance.CanvasGroups.Add(_canvasGroup);
    }

    private void Update()
    {
        if (Input.GetKeyDown(_dissableEnableScreen))
            PressEnableUIButton.Invoke();
    }

    /// <summary>
    /// When the activate button is pressed the UI will be shown
    /// </summary>
    private void ActivateUI() => _menuGFX.SetActive(!_menuGFX.activeSelf);
    
    /// <summary>
    /// Set the childpos to the last of the children so it is ontop of everything
    /// </summary>
    private void SetChildPos() => transform.SetAsLastSibling();
    
    /// <summary>
    /// When the ui is enabled make all the other menus not interactable
    /// </summary>
    private void DeactivateInteractables()
    {
        foreach (CanvasGroup group in UIManager.Instance.CanvasGroups)
            group.interactable = false;

        _canvasGroup.interactable = true;                   
    }
}
