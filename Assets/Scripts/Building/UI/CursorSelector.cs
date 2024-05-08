using System;
using System.Collections;
using System.Collections.Generic;
using Building.Datatypes;
using TMPro;
using UnityEngine;
using Cursor = Building.Cursor;

public class CursorSelector : MonoBehaviour
{
    [SerializeField] private List<BuildableObject> _buildable;

    private void Start()
    {
        GetComponent<TMP_Dropdown>().onValueChanged.AddListener(Select);
    }

    private void Select(int index)
    {
        FindObjectOfType<Cursor>().ChangeToBuilding(_buildable[index]);
    }
}