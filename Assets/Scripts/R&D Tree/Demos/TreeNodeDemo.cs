using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TreeNodeDemo : MonoBehaviour
{
    public TMP_Text Title;
    public TMP_Text Description;
    public Button ActivateButton;
    public Button QueueButton;
    public Slider slider;

    public ResearchNode researchNode;
    public RDTreeManager treeManager;

    bool addedEvents = false;

    private void Update()
    {
        if (addedEvents == false)
        {
            for (int i = 0; i < researchNode.NodeSetting.ConnectedResearchNodes.Count; i++)
            {
                researchNode.ResearchDoneEvent.AddListener(researchNode.NodeSetting.ConnectedResearchNodes[i].GetComponent<TreeNodeDemo>().NodeStates);
            }

            Title.text = researchNode.NodeSetting.Title;
            Description.text = researchNode.NodeSetting.Description;

            addedEvents = true;
        }

        NodeStates();
    }

    private void FixedUpdate()
    {
        slider.value = researchNode.ResearchValue;
    }

    public void NodeStates()
    {
        switch (researchNode.CurrentResearchState)
        {
            case ResearchNode.ResearchStates.notAvailable:
                ActivateButton.interactable = false;
                gameObject.SetActive(false);
                break;
            case ResearchNode.ResearchStates.available:
                ActivateButton.interactable = true;
                gameObject.SetActive(true);
                break;
            case ResearchNode.ResearchStates.inDevelopment:
                ActivateButton.interactable = false;
                break;
            case ResearchNode.ResearchStates.bought:
                ActivateButton.interactable = false;
                break;
        }

        if (!researchNode.IsResearchInQueue)
        {
            if (researchNode.CurrentResearchState == ResearchNode.ResearchStates.available)
                QueueButton.interactable = true;
            else
                QueueButton.interactable = false;
        }
        else
            QueueButton.interactable = false;
    }

    public void ClickActivateButton()
    {
        slider.maxValue = researchNode.NodeSetting.ResearchCompletionValue;
        researchNode.StartResearch();
        NodeStates();
    }

    public void ClickQueueButton()
    {
        slider.maxValue = researchNode.NodeSetting.ResearchCompletionValue;
        researchNode.AddResearchToQueue();
        NodeStates();
    }

}
