using UnityEngine;

public class MenuDemo : MonoBehaviour
{
    [SerializeField] private KeyCode _enable;
    [SerializeField] private KeyCode _disable;

    [SerializeField] private UIElementInteractable _interactable;
    [SerializeField] private GameObject _uiOBJ;
    [SerializeField] private InteractableManager _manager;

    private void Update()
    {
        if (Input.GetKeyDown(_enable))
        {
            _interactable.EnableInteraction();

            foreach(IInteractable interactable in _manager.Interactables)
            {
                if (_interactable == interactable) continue;

                interactable.DisableInteraction();
            }
            /*foreach (IInteractable interactbale in _manager.Interactables)
            {
                if (_interactable == interactbale) continue;
                
                interactbale.DisableInteraction();
            }*/
            _uiOBJ.SetActive(true);
        }

        if (Input.GetKeyDown(_disable))
        {
            _interactable.DisableInteraction();

            _manager.Interactables[0].EnableInteraction();

            _uiOBJ.SetActive(false);
        }
    }
}
