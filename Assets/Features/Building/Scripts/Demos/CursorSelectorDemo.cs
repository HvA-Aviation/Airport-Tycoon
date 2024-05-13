using System.Collections.Generic;
using Features.Building.Scripts.Datatypes;
using TMPro;
using UnityEngine;
using Cursor = Features.Building.Scripts.Grid.Cursor;

namespace Features.Building.Scripts.Demos
{
    public class CursorSelectorDemo : MonoBehaviour
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

    }
}