using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ResearchNode : MonoBehaviour
{
    [SerializeField] private RDTreeManager _treeManager;
    public float ResearchValue { get; private set; }

    public delegate void ResearchDoneEvent();
    public ResearchDoneEvent ResearchDone;

    public ResearchNodeSetting NodeSetting {  get; private set; }

    public ResearchStates CurrentResearchState { get;  private set; }

    private void Start()
    {
        NodeSetting = GetComponent<ResearchNodeSetting>();
        _treeManager.AllNodes.Add(this);    

        ResearchDone += SetResearchBought;
        ResearchDone += SetNextStatesInTree;
        ResearchDone += NodeSetting.UnlockObjects;
        ResearchDone += _treeManager.ResearchFinished;
    }

    /// <summary>
    /// This function is called to set the next nodes active when the research is done
    /// </summary>
    private void SetNextStatesInTree()
    {
        foreach (var connectedNode in NodeSetting.ConnectedResearchNodes)
        {
            if (connectedNode == null) 
                continue;

            connectedNode.CurrentResearchState = ResearchStates.available;
            connectedNode.GetComponent<TreeNodeDemo>().NodeStates();
        }
    }

    /// <summary>
    /// Call this function when you want the research to be developed
    /// </summary>
    /// <param name="value">The amount you want to add to the researchvalue
    /// </param>
    public void AddValue(float value)
    {
        if (CurrentResearchState != ResearchStates.inDevelopment) 
            return;

        ResearchValue += value;

        if (ResearchValue >= NodeSetting.ResearchCompletionValue)
        {
            if (ResearchDone != null)
                ResearchDone();
        }
    }

    /// <summary>
    /// This function is called to pause the research or to set nodes available
    /// </summary>
    public void SetAvailableAvailable() => CurrentResearchState = ResearchStates.available;
   
    /// <summary>
    /// This function is called to set the research in development
    /// </summary>
    public void SetResearchInDevelopment() => CurrentResearchState = ResearchStates.inDevelopment;

    /// <summary>
    /// This function will be called when the research states need to be set to bought
    /// </summary>
    public void SetResearchBought() => CurrentResearchState = ResearchStates.bought;

    /// <summary>
    /// This function is called when you want to start the research
    /// </summary>
    public void StartResearch() => _treeManager.StartNewResearch(this);    

    /// <summary>
    /// All the states for the research
    /// </summary>
    public enum ResearchStates { notAvailable, available, inDevelopment, bought }
}
