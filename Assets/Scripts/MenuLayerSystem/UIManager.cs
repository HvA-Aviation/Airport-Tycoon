using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public List<IInteractable> Interactables { get; set; }

    private void Awake()
    {
        Interactables = new List<IInteractable>();
    }
}
