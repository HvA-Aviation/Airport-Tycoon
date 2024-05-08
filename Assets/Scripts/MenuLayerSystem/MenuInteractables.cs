using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class MenuInteractables : MonoBehaviour
{
    private CanvasGroup _canvasGroup;

    private void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();

        UIManager.Instance.CanvasGroups.Add(_canvasGroup);
    }
}
