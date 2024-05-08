using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreeNodeDemo : MonoBehaviour
{
    public Button ActivateButton;
    public Button QueueButton;
    public Slider slider;

    public ResearchNode researchNode;
    public ResearchNodeQueue researchQueue;
    public RDTreeManager treeManager;

    private void Start()
    {
        NodeStates();
        QueueStates();
    }

    private void Update()
    {
        NodeStates();
    }

    private void FixedUpdate()
    {
        slider.value = researchNode.ResearchTimer;
    }

    public void NodeStates()
    {
        switch (researchNode.CurrentSkillState)
        {
            case ResearchNode.SkillState.notAvailable:
                ActivateButton.interactable = false;
                gameObject.SetActive(false);
                break;
            case ResearchNode.SkillState.available:
                ActivateButton.interactable = true;
                gameObject.SetActive(true);
                break;
            case ResearchNode.SkillState.inDevelopment:
                ActivateButton.interactable = false;
                break;
            case ResearchNode.SkillState.bought:
                ActivateButton.interactable = false;
                break;
        }
    }

    public void QueueStates()
    {
        switch (researchQueue.CurrentState)
        {
            case ResearchNodeQueue.QueueStates.NotInQueue:
                QueueButton.interactable = true;
                break;
            case ResearchNodeQueue.QueueStates.InQueue:
                QueueButton.interactable = false;
                break;
            case ResearchNodeQueue.QueueStates.FinishedQueue:
                QueueButton.interactable = false;
                break;
        }
    }
    public void ClickActivateButton()
    {
        slider.maxValue = researchNode.NodeSetting.ResearchTime;
        researchNode.StartResearch();
        NodeStates();
    }

    public void ClickQueueButton()
    {
        slider.maxValue = researchNode.NodeSetting.ResearchTime;
        researchQueue.AddNodeToQueue();
        QueueStates();
    }

}
