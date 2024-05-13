using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RDTreeManager : MonoBehaviour
{
    [SerializeField] private GameObject _tree;
    [SerializeField] private List<ResearchNode> _firstAvailableNodes = new List<ResearchNode>();

    public ResearchNode CurrentResearching { get; private set; }

    public List<ResearchNode> ResearchQueue = new List<ResearchNode>();

    private void Start() => ActivateFirstNodes();    

    private void FixedUpdate() => CurrentResearching?.AddTime(Time.fixedDeltaTime);   

    /// <summary>
    /// This function sets the firstavailable nodes in the tree active and available
    /// </summary>
    private void ActivateFirstNodes()
    {
        foreach(ResearchNode node in _firstAvailableNodes)
        {
            node.SetNodeAvailable();
            node.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// This function will be called when the player wants to show the tree
    /// </summary>
    public void ShowTree() => _tree?.SetActive(!_tree.activeSelf);

    /// <summary>
    /// Checks if there is an node in the queue and returns that node
    /// </summary>
    /// <returns></returns>
    public ResearchNode NextInQueue()
    {
        if (ResearchQueue.Count > 0)
            return ResearchQueue[0];
        else
            return null;
    }    
    
    /// <summary>
    /// This method will pause the research of the one that is currently researching and then it will set the new research.
    /// </summary>
    /// <param name="newResearch">The research that needs to be started</param>
    public void ChooseNewResearch(ResearchNode newResearch)
    {
        if (CurrentResearching != null)
            CurrentResearching.SetNodeAvailable();

        CurrentResearching = newResearch;
    }

    /// <summary>
    /// This method needs to be called when you want to add the research to the queue
    /// </summary>zs
    /// <param name="research">The research that needs to be addeed to the queue</param>
    public void AddResearchToQueue(ResearchNode research)
    {
        if (ResearchQueue.Contains(research)) return;
        
        ResearchQueue.Add(research);
    }
    /// <summary>
    /// Call this function when the research is finished researching
    /// It will look if there is a node in the queue and set the research to that node
    /// </summary>
    public void ResearchFinished() 
    {
        CurrentResearching = null;
        CurrentResearching = NextInQueue();
        CurrentResearching?.StartResearch();
    }
}
