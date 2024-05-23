using Features.Managers;
using TMPro;
using UnityEngine;
using Cursor = Features.Building.Scripts.Grid.Cursor;

namespace Features.Building.Scripts.Demos
{
    public class CursorSelectorDemo : MonoBehaviour
    {
        [SerializeField] private Cursor _cursor;
        [SerializeField] private TMP_Dropdown _dropdown;

        private void Start()
        {
            foreach (BuildingStatus buildingStatus in GameManager.Instance.BuildingManager.BuildingStatuses)
            {
                _dropdown.options.Add(new TMP_Dropdown.OptionData(buildingStatus.BuildableObject.name, buildingStatus.BuildableObject.BuildItems[0].Tile.sprite));
            }

            _dropdown.onValueChanged.AddListener(Select);
            _dropdown.RefreshShownValue();
        }

        private void Select(int index)
        {
            GameManager.Instance.BuildingManager.ChangeSelectedBuildable(GameManager.Instance.BuildingManager.BuildingStatuses[index].BuildableObject);
        }
    }
}