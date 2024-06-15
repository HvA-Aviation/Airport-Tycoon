using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class UIElementInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private InteractableManager _uiManager;

    private CanvasGroup _canvasGroup;

    private void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();

        _uiManager.AddInteractableToList(this);
    }

    /// <summary>
    /// This function is called when you want to disable an UI element
    /// </summary>
    public void DisableInteraction()
    {
        _uiManager.PutElementLastInList(this);
        _canvasGroup.interactable = false;
    }

    /// <summary>
    /// This function is called when you want to enable an UI element
    /// </summary>
    public void EnableInteraction()
    {
        _uiManager.PutElementToFirstInList(this);
        transform.SetAsLastSibling();
        _canvasGroup.interactable = true;
    }
}
