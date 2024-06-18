using UnityEngine;
using Features.Managers;

public class ShowQueueUIDemo : MonoBehaviour
{
    public GameObject finishQueueButton;

    void Start()
    {
        GameManager.Instance.EventManager.Subscribe(Features.EventManager.EventId.OnBuildingQueue, (args) => ShowQueue());
    }

    public void ShowQueue()
    {
        print(!finishQueueButton.activeSelf);
        finishQueueButton.SetActive(!finishQueueButton.activeSelf);
    }
}
