using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Features.Managers;
using Features.Building.Scripts.Datatypes;
using Features.Building.Scripts.Datatypes.TileData;
using System.Linq;

public class UIDataHandler : MonoBehaviour
{
    [Header("Terminal Management/Passenger Summary")]
    public TMP_Text PassengerAmount;
    public TMP_Text TimeSpentInAirport;
    public TMP_Text AmountOfPaxEntered;
    public TMP_Text TotalAmountOfPaxOnPlane;

    [Header("Terminal Management/Average Queue Time")]
    public Transform QueueInfoPanel;
    public GameObject QueueInfoPanelPrefab;
    private Dictionary<Vector3Int, GameObject> ExistingQueueUI = new();
    public BuildableAtlas buildableAtlas;

    [Header("Bottom Nav/DayMonthYear")]
    public DayCycle dayCycleReference;
    public TMP_Text dayMonthYear;

    void OnEnable() => Invoke(nameof(StartTracking), .5f);

    void OnDisable() => StopAllCoroutines();

    private void StartTracking() => StartCoroutine(UpdateUI());

    IEnumerator UpdateUI()
    {
        while (true)
        {
            HandlePassengerSummary();

            HandleDayMonthYear();

            HandleAverageQueueTime();

            yield return new WaitForSecondsRealtime(1f);
        }
    }

    private void HandlePassengerSummary()
    {
        PassengerAmount.text = GameManager.Instance.PaxManager.AmountOfPaxInAirport.ToString();

        float averageTimeInAirport = GameManager.Instance.PaxManager.AverageTimeInAirport;

        TimeSpentInAirport.text = averageTimeInAirport == 0 ? "No Data Available" : averageTimeInAirport.ToString("0.00") + " Minutes";

        AmountOfPaxEntered.text = GameManager.Instance.PaxManager.AmountOfPaxEntered.ToString();
        TotalAmountOfPaxOnPlane.text = GameManager.Instance.PaxManager.TotalAmountOfRecordedPax.ToString();
    }

    private void HandleAverageQueueTime()
    {
        var list = GameManager.Instance.QueueManager.QueueTimeEstimate;

        foreach (var item in list)
        {
            if (!ExistingQueueUI.TryGetValue(item.Key, out var value))
            {
                value = Instantiate(QueueInfoPanelPrefab, QueueInfoPanel);
                ExistingQueueUI.Add(item.Key, value);
            }

            var index = GameManager.Instance.GridManager.Grid.Get(item.Key);
            bool isValid = buildableAtlas.TileIsType<UtilityTile>(index != -1 ? index : 0);

            // if key is no longer valid then it has been removed, remove it from the list so it no longer updates or shows up
            if (!isValid)
            {
                Destroy(ExistingQueueUI[item.Key]);
                ExistingQueueUI.Remove(item.Key);
                continue;
            }

            UtilityType utilityType = buildableAtlas.GetTileData<UtilityTile>(index).UtilityType;

            value.GetComponent<InformationSetter>().SetInformation(utilityType.ToString(), item.Value.ToString());
        }
    }

    private void HandleDayMonthYear()
    {
        dayMonthYear.text = dayCycleReference.GetDayMonthDay;
    }
}
