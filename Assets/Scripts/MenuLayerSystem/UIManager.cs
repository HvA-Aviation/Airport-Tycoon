using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public List<CanvasGroup> CanvasGroups { get; set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(Instance);
        
        CanvasGroups = new List<CanvasGroup>();
    }
}
