using UnityEngine;

public class MenuDemo : MonoBehaviour
{
    [SerializeField] private KeyCode _enable;
    [SerializeField] private KeyCode _disable;

    [SerializeField] private UIElementInteractable _interactable;
    [SerializeField] private GameObject _uiOBJ;
    [SerializeField] private UIManager _manager;

    private void Update()
    {
        if (Input.GetKeyDown(_enable))
        {
            _interactable.EnableInteraction();

            foreach (IInteractable interactbale in _manager.Interactables)
            {
                if (interactbale == _interactable) continue;
                else interactbale.DissableInteraction();
            }
            _uiOBJ.SetActive(true);
        }

        if (Input.GetKeyDown(_disable))
        {
            _interactable.DissableInteraction();

            transform.parent.GetChild(_manager.Interactables.Count - 1).GetComponent<IInteractable>().EnableInteraction();

            _uiOBJ.SetActive(false);
        }
    }
}
