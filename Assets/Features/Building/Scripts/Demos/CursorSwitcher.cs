using System.Collections.Generic;
using Features.EventManager;
using Features.Managers;
using UnityEngine;

public enum CursorMode
{
    BUILDING,
    SELECTING
}

public class CursorSwitcher : MonoBehaviour
{
    [SerializeField] private CursorMode _currentCursorMode = CursorMode.BUILDING;

    [Header("Components belonging to building mode"), Space(5)]
    [Tooltip("Fill this list with monobehaviours that handle building tiles")]
    [SerializeField] private List<MonoBehaviour> ListOfBuildingComponents;

    [Header("Components belonging to selecting mode"), Space(5)]
    [Tooltip("Fill this list with monobehaviours that handle selecting tiles")]
    [SerializeField] private List<MonoBehaviour> ListOfSelectingComponents;

    private Dictionary<CursorMode, List<MonoBehaviour>> _cursorModeComponents = new Dictionary<CursorMode, List<MonoBehaviour>>{
        {CursorMode.BUILDING, new List<MonoBehaviour>()},
        {CursorMode.SELECTING, new List<MonoBehaviour>()}
    };

    private void Start()
    {
        _cursorModeComponents[CursorMode.BUILDING] = ListOfBuildingComponents;
        _cursorModeComponents[CursorMode.SELECTING] = ListOfSelectingComponents;

        GameManager.Instance.EventManager.Subscribe(EventId.OnCursorSwitch, (args) => SwitchCursor());
    }

    /// <summary>
    /// Switches the cursor mode between building and selecting.
    /// </summary>
    public void SwitchCursor()
    {
        foreach (var value in _cursorModeComponents.Values)
        {
            foreach (var component in value)
            {
                component.enabled = false;
            }
        }

        _currentCursorMode = _currentCursorMode == CursorMode.BUILDING ? CursorMode.SELECTING : CursorMode.BUILDING;

        EnableCursorModeComponents(_currentCursorMode);
    }

    /// <summary>
    /// Enables the components associated with the specified cursor mode.
    /// </summary>
    /// <param name="cursorMode">The cursor mode to enable components for.</param>
    private void EnableCursorModeComponents(CursorMode cursorMode)
    {
        _cursorModeComponents.TryGetValue(cursorMode, out List<MonoBehaviour> selectingComponents);

        foreach (MonoBehaviour item in selectingComponents)
            item.enabled = true;
    }
}
