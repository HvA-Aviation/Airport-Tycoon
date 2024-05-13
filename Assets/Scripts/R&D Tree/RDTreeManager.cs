using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RDTreeManager : MonoBehaviour
{
    [SerializeField] private List<ResearchNode> _firstAvailableNodes = new List<ResearchNode>();
     
    public ResearchNode CurrentResearching { get; private set; }
    public List<ResearchNode> AllNodes { get; set; }
    public List<ResearchNode> ResearchQueue {  get; private set; }

    private void Awake()
    {
        AllNodes = new List<ResearchNode>();
        ResearchQueue = new List<ResearchNode>();
    }

    private void Start() => ActivateFirstNodes();    

    private void FixedUpdate() => CurrentResearching?.AddValue(1);   

    /// <summary>
    /// This function sets the firstavailable nodes in the tree active and available
    /// </summary>
    private void ActivateFirstNodes()
    {
        foreach(ResearchNode node in _firstAvailableNodes)
        {
            node.SetAvailableAvailable();
        }
    }

    /// <summary>
    /// Function that returns a list of all the nodes that are available to buy
    /// </summary>
    /// <returns>Return a list with all the available nodes</returns>
    public List<ResearchNode> AvailableNodes()
    {
        var availableNodes = new List<ResearchNode>();

        foreach(var node in AllNodes)
        {
            if(node.CurrentResearchState == ResearchNode.ResearchStates.available)
                availableNodes.Add(node);
        }

        return availableNodes;
    }    

    /// <summary>
    /// Checks if there is an node in the queue and returns that node
    /// </summary>
    /// <returns>Returns the next node in the queue</returns>
    public ResearchNode NextInQueue()
    {
        if (ResearchQueue.Count > 0)
            return ResearchQueue[0];
        else
            return null;
    }    
    
    public void StartNewResearch(ResearchNode research)
    {
        if(research.CurrentResearchState != ResearchNode.ResearchStates.available)
            return;

        ChooseNewResearch(research);
        research.SetResearchInDevelopment();
    }

    /// <summary>
    /// This method will pause the research of the one that is currently researching and then it will set the new research.
    /// </summary>
    /// <param name="newResearch">The research that needs to be started</param>
    public void ChooseNewResearch(ResearchNode newResearch)
    {
        if (CurrentResearching != null)
            CurrentResearching.SetAvailableAvailable();

        CurrentResearching = newResearch;
    }

    /// <summary>
    /// This method needs to be called when you want to add the research to the queue
    /// </summary>
    /// <param name="research">The research that needs to be addeed to the queue</param>
    public void AddResearchToQueue(ResearchNode research)
    {
        if (ResearchQueue.Contains(research))
            return;     
        
        ResearchQueue.Add(research);

        //Check to see if research needs to start when it is added in the queue
        if (CurrentResearching == null)
            ResearchFinished();
    }

    /// <summary>
    /// This method nees to be called when you want to remove a researchnode from the queue
    /// </summary>
    /// <param name="research"></param>
    public void RemoveResearchFromQueue(ResearchNode research)
    {
        if (!research.IsResearchInQueue && !ResearchQueue.Contains(research))
            return;

        ResearchQueue.Remove(research);
    }

    /// <summary>
    /// Call this function when the research is finished researching
    /// It will look if there is a node in the queue and set the research to that node
    /// </summary>
    public void ResearchFinished() 
    {
        CurrentResearching = NextInQueue();
        CurrentResearching?.StartResearch();
    }
}
