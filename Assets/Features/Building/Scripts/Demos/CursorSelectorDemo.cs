using System.Linq;
using Features.EventManager;
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
            UpdateDropDown();

            _dropdown.onValueChanged.AddListener(Select);
            _dropdown.RefreshShownValue();

            GameManager.Instance.EventManager.Subscribe(EventId.OnUnlockBuilding, (args) => UpdateDropDown());
        }

        private void UpdateDropDown()
        {
            _dropdown.options.Clear();

            foreach (BuildingStatus buildingStatus in GameManager.Instance.BuildingManager.BuildingStatuses.Where(x =>
                         x.IsUnlocked))
            {
                string addition = buildingStatus.BuildableObject.Price > 0
                    ? " [" + buildingStatus.BuildableObject.Price + "]"
                    : "";

                _dropdown.options.Add(new TMP_Dropdown.OptionData(buildingStatus.BuildableObject.name + addition,
                    buildingStatus.BuildableObject.BuildItems[0].Tile.sprite));
            }
        }

        private void Select(int index)
        {
            GameManager.Instance.BuildingManager.ChangeSelectedBuildable(index);
        }
    }
}