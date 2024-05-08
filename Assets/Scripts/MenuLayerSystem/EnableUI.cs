using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnableUI : MonoBehaviour
{
    [SerializeField] private KeyCode _dissableEnableScreen;
    [SerializeField] private GameObject _menuGFX;

    public UnityEvent PressEnableUIButton = new UnityEvent();

    private void Awake()
    {
        PressEnableUIButton.AddListener(ActivateUI);
        PressEnableUIButton.AddListener(SetChildPos);
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
    
}
