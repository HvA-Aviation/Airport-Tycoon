using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public List<CanvasGroup> CanvasGroups { get; set; }

    private void Awake()
    {        
        CanvasGroups = new List<CanvasGroup>();
    }
}
