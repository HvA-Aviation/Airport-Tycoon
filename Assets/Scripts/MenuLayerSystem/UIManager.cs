using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public List<IInteractable> Interactables { get; private set; }

    private void Awake()
    {
        Interactables = new List<IInteractable>();
    }

    public void AddInteractableToList(IInteractable interactable) => Interactables.Add(interactable);
}
