using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ResearchNode : MonoBehaviour
{
    [SerializeField] private RDTreeManager _treeManager;
    public float ResearchValue { get; private set; }

    public bool IsResearchInQueue { get; private set; } 

    public ResearchNodeSetting NodeSetting {  get; private set; }

    public ResearchStates CurrentResearchState { get;  private set; }

    [HideInInspector]public UnityEvent ResearchDoneEvent;

    private void Start()
    {
        NodeSetting = GetComponent<ResearchNodeSetting>();
        _treeManager.AllNodes.Add(this);    
    }   

    /// <summary>
    /// This function is called when the research is finished
    /// </summary>
    public void ResearchDone()
    {
        ResearchDoneEvent?.Invoke();
        SetResearchBought();
        SetNextStatesInTree();
        RemoveResearchFromQueue();
        _treeManager.ResearchFinished();
    }

    /// <summary>
    /// This function is called to set the next nodes active when the research is done
    /// </summary>
    public void SetNextStatesInTree()
    {
        foreach (var connectedNode in NodeSetting.ConnectedResearchNodes)
        {
            if (connectedNode == null) 
                continue;

            connectedNode.CurrentResearchState = ResearchStates.available;

            //Deleted this code when the visualisation of the tree is being made
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
            ResearchDone();
    }
    
    public void SetResearchNotAvailable() => CurrentResearchState = ResearchStates.notAvailable;

    public void SetAvailableAvailable() => CurrentResearchState = ResearchStates.available;
       
    public void SetResearchInDevelopment() => CurrentResearchState = ResearchStates.inDevelopment;
       
    public void SetResearchBought() => CurrentResearchState = ResearchStates.bought;

    /// <summary>
    /// This function is called when you want to start the research
    /// </summary>
    public void StartResearch() => _treeManager.StartNewResearch(this);

    /// <summary>
    /// This function is called when the research is finished
    /// The research will then be removed from the queue
    /// </summary>
    public void RemoveResearchFromQueue() 
    {
        IsResearchInQueue = false;
        _treeManager.RemoveResearchFromQueue(this); 
    }

    /// <summary>
    /// This function will be called when you want to add research to the queue
    /// </summary>
    public void AddResearchToQueue() 
    { 
        IsResearchInQueue = true;
        _treeManager.AddResearchToQueue(this); 
    }

    /// <summary>
    /// All the states for the research
    /// </summary>
    public enum ResearchStates { notAvailable, available, inDevelopment, bought }
}
