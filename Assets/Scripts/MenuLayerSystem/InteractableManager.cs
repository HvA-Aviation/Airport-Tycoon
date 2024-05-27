using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InteractableManager : MonoBehaviour
{
    public List<IInteractable> Interactables { get; private set; }

    private void Awake()
    {
        Interactables = new List<IInteractable>();
    }

    public void AddInteractableToList(IInteractable interactable) => Interactables.Add(interactable);

    /// <summary>
    /// This function will put the interactable element in the first position on the list and then reorder the list
    /// </summary>
    /// <param name="interactable"> The interactable element that needs to be put first</param>
    public void PutElementToFirstInList(IInteractable interactable)
    {
        Interactables.Remove(interactable);

        Interactables.Insert(0, interactable);
    }

    /// <summary>
    /// This function will put the interactable element in the last position in the list and reorder the list
    /// </summary>
    /// <param name="interactable">The interactable that needs to be put on the last position</param>
    public void PutElementLastInList(IInteractable interactable)
    {
        List<IInteractable> tempList = new List<IInteractable>();

        tempList = Interactables.Where(e => e != interactable).ToList();

        tempList.Add(interactable);

        Interactables = tempList;
    }
}
