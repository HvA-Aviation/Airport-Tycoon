using System;
using System.Collections.Generic;
using System.Linq;
using Features.Building.Scripts.Datatypes;
using Features.Building.Scripts.Datatypes.TileData;
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
    [SerializeField] private GameObject hoverTile;
    private Vector3Int mousePosition;

    void OnEnable() => hoverTile.SetActive(true);

    void OnDisable() => hoverTile.SetActive(false);

    // Update is called once per frame
    void FixedUpdate()
    {
        mousePosition = Vector3Int.RoundToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        mousePosition.x = Mathf.Clamp(mousePosition.x, 0, _grid.GridSize.x);
        mousePosition.y = Mathf.Clamp(mousePosition.y, 0, _grid.GridSize.y);
        mousePosition.z = 0;

        hoverTile.transform.position = mousePosition;
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0) && !_eventSystem.IsPointerOverGameObject())
        {
            print("pressed lmb");
            SelectTile();
        }
    }

    private void SelectTile()
    {
        for (int i = 0; i < Enum.GetNames(typeof(GridLayer)).Length; i++)
        {
            mousePosition.z = i;

            switch ((GridLayer)mousePosition.z)
            {
                case GridLayer.Floor:
                    break;
                case GridLayer.Objects:
                    HandleObjectSelection();
                    break;
                case GridLayer.Zone:
                    break;
            }
        }
    }

    void HandleObjectSelection()
    {
        int indexValue = _grid.Get(mousePosition);

        if (indexValue == BuildableAtlas.Empty)
        {
            print("Tile is empty");
            return;
        }

        UtilityType utilityTile = _atlas.GetTileData<UtilityTile>(indexValue).UtilityType;

        if (_grid.GetUtilities(utilityTile, out Dictionary<Vector3Int, List<Vector3Int>> utilities))
        {
            foreach (var item in utilities)
            {
                if (item.Key == mousePosition && item.Value.Count > 0)
                {
                    _grid.InitializeQueueBuilder(new List<Vector3Int> { mousePosition });
                    GameManager.Instance.EventManager.TriggerEvent(Features.EventManager.EventId.OnCursorSwitch);
                }
            }
        }
    }
}
