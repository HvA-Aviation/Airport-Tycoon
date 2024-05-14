using UnityEngine;
using UnityEngine.Events;

public class ResearchNode : MonoBehaviour
{
    [SerializeField] private RDTreeManager _treeManager;
    public float ResearchValue { get; private set; }

    public bool IsResearchInQueue { get; private set; }

    public ResearchNodeSetting NodeSetting { get; private set; }

    public ResearchStates CurrentResearchState { get; private set; }

    [HideInInspector] public UnityEvent ResearchDoneEvent;

    private void Awake()
    {
        NodeSetting = GetComponent<ResearchNodeSetting>();
        _treeManager?.AddNodeToList(this);
    }

    /// <summary>
    /// This function is called when the research is finished
    /// </summary>
    public void ResearchDone()
    {
        SetResearchStatus(ResearchStates.bought);
        SetNextStatesInTree();
        RemoveResearchFromQueue();
        ResearchDoneEvent?.Invoke();
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

    /// <summary>
    /// Call this function when you want to change the state of the research
    /// </summary>
    /// <param name="state">The state you want the research to be in</param>
    public void SetResearchStatus(ResearchStates state) => CurrentResearchState = state;

    /// <summary>
    /// This function is called when you want to start the research
    /// </summary>
    public void StartResearch() => _treeManager.ChooseNewResearch(this);

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
