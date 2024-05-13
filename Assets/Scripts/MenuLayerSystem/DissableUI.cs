using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class DissableUI : MonoBehaviour
{
    [SerializeField] private GameObject _menuGFX;
    [SerializeField] private UIManager _uiManager;
    [HideInInspector] public UnityEvent PressDissableUIButton = new UnityEvent();
    private void Awake()
    {
        PressDissableUIButton.AddListener(HideUI);
        PressDissableUIButton.AddListener(PutScreenBelow);
        PressDissableUIButton.AddListener(SetNewInteractable);
    }

    /// <summary>
    /// When the deactivate button is pressed the UI will not be able to be seen
    /// </summary>
    private void HideUI() => _menuGFX.SetActive(false);

    /// <summary>
    /// Set the childpos to the first position of the children so it is below of everything
    /// </summary>
    private void PutScreenBelow() => transform.SetAsFirstSibling();

    /// <summary>
    /// When the UI is closed set the UI ontop to be able to interact
    /// </summary>
    private void SetNewInteractable()
    {
        var child = transform.parent.GetChild(_uiManager.Interactables.Count - 1);

        if (child.TryGetComponent<CanvasGroup>(out CanvasGroup thisGroup))
            thisGroup.interactable = true;
    }
}
