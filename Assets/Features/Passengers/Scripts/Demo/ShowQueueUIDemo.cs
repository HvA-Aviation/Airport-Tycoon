using UnityEngine;
using Features.Managers;

public class ShowQueueUIDemo : MonoBehaviour
{
    public GameObject finishQueueButton;

    void Start()
    {
        finishQueueButton.SetActive(false);
        GameManager.Instance.EventManager.Subscribe(Features.EventManager.EventId.OnBuildingQueue, (args) => ShowQueue());
    }

    public void ShowQueue()
    {
        finishQueueButton.SetActive(!finishQueueButton.activeSelf);
    }
}
