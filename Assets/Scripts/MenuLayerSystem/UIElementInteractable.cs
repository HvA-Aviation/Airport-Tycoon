using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class UIElementInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private InteractableManager _uiManager;

    private CanvasGroup _thisCanvasGroup;

    private void Start()
    {
        _thisCanvasGroup = GetComponent<CanvasGroup>();

        _uiManager.AddInteractableToList(this);
    }

    public void DissableInteraction()
    {
        _uiManager.PutElementLastInList(this);
        _thisCanvasGroup.interactable = false;
    }

    public void EnableInteraction()
    {
        _uiManager.PutElementToFirstInList(this);
        transform.SetAsLastSibling();
        _thisCanvasGroup.interactable = true;
    }
}
