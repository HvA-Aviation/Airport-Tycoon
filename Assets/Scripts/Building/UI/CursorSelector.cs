using System;
using System.Collections;
using System.Collections.Generic;
using Building.Datatypes;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using Cursor = Building.Cursor;

public class CursorSelector : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private List<BuildableObject> _buildable;
    [SerializeField] private Cursor _cursor;

    private void Start()
    {
        GetComponent<TMP_Dropdown>().onValueChanged.AddListener(Select);
        
    }

    private void Select(int index)
    {
        _cursor.ChangeToBuilding(_buildable[index]);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _cursor.DisableCursor();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _cursor.EnableCursor();
    }
}