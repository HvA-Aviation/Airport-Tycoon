using UnityEngine;

public class NoUIElementInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private InteractableManager _uiManager;

    private bool _isInteractable = true;

    private void Start()
    {
        _uiManager.AddInteractableToList(this);
    }

    public void DisableInteraction()
    {
        _uiManager.PutElementLastInList(this);
        _isInteractable = false;
    }

    public void EnableInteraction()
    {
        _uiManager.PutElementToFirstInList(this);
        _isInteractable = true;
    }

    public void Update()
    {
        if (_isInteractable && Input.GetKeyDown(KeyCode.Space))
            Debug.Log("Interactable");

    }
}
