using System.Collections.Generic;
using UnityEngine;

public class RDTreeManager : MonoBehaviour
{
    public ResearchNode CurrentResearching { get; private set; }

    public List<ResearchNode> ResearchQueue = new List<ResearchNode>();

    private void FixedUpdate()
    {
        CurrentResearching?.AddTime(Time.fixedDeltaTime);
    }

    /// <summary>
    /// Checks if there is an node in the queue and returns that node
    /// </summary>
    /// <returns></returns>
    public ResearchNode NextInQueue()
    {
        if (ResearchQueue.Count > 0 && CurrentResearching == null)
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
            CurrentResearching.PauseResearch();

        CurrentResearching = newResearch;
    }

    /// <summary>
    /// Call this function when the research is finished researching
    /// It will look if there is a node in the queue and set the research to that node
    /// </summary>
    public void ResearchFinished() 
    { 
        CurrentResearching = NextInQueue();
    }
}
