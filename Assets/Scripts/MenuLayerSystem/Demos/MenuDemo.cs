using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuDemo : MonoBehaviour
{
    [SerializeField] private KeyCode _enable;
    [SerializeField] private KeyCode _disable;

    [SerializeField] private MenuInteractable _interactable;
    [SerializeField] private GameObject _uiOBJ;
    [SerializeField] private UIManager _manager;

    private void Update()
    {
        if (Input.GetKeyDown(_enable))
        {
            foreach(var interactable in _manager.Interactables)
            {
                if(interactable == _interactable)                
                    interactable.EnableInteraction();                
                else
                    interactable.DissableInteraction();
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
