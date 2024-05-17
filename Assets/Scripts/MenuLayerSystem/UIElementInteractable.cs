using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class UIElementInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private UIManager _uiManager;

    private CanvasGroup _thisCanvasGroup;

    private void Start()
    {
        _thisCanvasGroup = GetComponent<CanvasGroup>();
        _uiManager.Interactables.Add(this);
    }

    public void DissableInteraction()
    {
        if (transform == transform.parent.GetChild(_uiManager.Interactables.Count - 1)) transform.SetAsFirstSibling();
        _thisCanvasGroup.interactable = false;
    }

    public void EnableInteraction()
    {
        transform.SetAsLastSibling();
        _thisCanvasGroup.interactable = true;
    }
}
