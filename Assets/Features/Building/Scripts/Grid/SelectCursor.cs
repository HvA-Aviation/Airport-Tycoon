using System;
using System.Collections.Generic;
using Features.Building.Scripts.Datatypes;
using Features.Building.Scripts.Grid;
using Features.Managers;
using UnityEngine;
using UnityEngine.EventSystems;
using Grid = Features.Building.Scripts.Grid.Grid;

public class SelectCursor : MonoBehaviour
{
    [SerializeField] private Grid _grid;
    [SerializeField] private EventSystem _eventSystem;
    [SerializeField] private BuildableAtlas _atlas;
    private Vector3Int mousePosition;

    enum MouseButtons
    {
        LEFT,
        RIGHT,
        MIDDLE,
    }

    // Update is called once per frame
    void Update()
    {
        mousePosition = Vector3Int.RoundToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        mousePosition.x = Mathf.Clamp(mousePosition.x, 0, _grid.GridSize.x);
        mousePosition.y = Mathf.Clamp(mousePosition.y, 0, _grid.GridSize.y);
        mousePosition.z = 0;

        if (Input.GetMouseButtonDown((int)MouseButtons.LEFT))
        {
            print("pressed lmb");
            SelectTile();
        }

        if (Input.GetMouseButtonDown((int)MouseButtons.MIDDLE))
        {
            print("pressed mmb");
        }

        if (Input.GetMouseButtonDown((int)MouseButtons.RIGHT))
        {
            print("pressed rmb");
        }
    }

    private void SelectTile()
    {
        for (int i = 0; i < Enum.GetNames(typeof(GridLayer)).Length; i++)
        {
            mousePosition.z = i;

            if (i == (int)GridLayer.Objects)
            {
                int indexValue = _grid.Get(mousePosition);

                if (indexValue == BuildableAtlas.Empty)
                {
                    print("Tile is empty");
                    continue;
                }

                Dictionary<Vector3Int, List<Vector3Int>> utilites = _grid.GetUtilities(_atlas.Items[_grid.Get(mousePosition)].UtilityType);
                foreach (var item in utilites)
                {
                    if (item.Key == mousePosition)
                    {
                        if (item.Value.Count > 0)
                        {
                            _grid.SwitchToQueueEditorExternal(mousePosition);
                        }
                    }
                }
            }
        }
    }
}
