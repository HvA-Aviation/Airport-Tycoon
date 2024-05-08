using UnityEngine;
using UnityEngine.UI;

public class ResearchNodeQueue : MonoBehaviour
{
    [SerializeField] private ResearchNode _researchNode;
    [SerializeField] private RDTreeManager _treeManager;

    public QueueStates CurrentState { get; private set; }

    private void Start()
    {
        _researchNode.ResearchDone += RemoveFromQueue;
    }

    /// <summary>
    /// Call this function when you want to add the node to the queue
    /// </summary>
    public void AddNodeToQueue() 
    {
        _treeManager.ResearchQueue.Add(_researchNode);
        CurrentState = QueueStates.InQueue;
        _researchNode.CurrentSkillState = ResearchNode.SkillState.inDevelopment;
        if (_treeManager.CurrentResearching == null)
            _treeManager.ResearchFinished();
    }

    /// <summary>
    /// This function will be called when the research is done.
    /// </summary>
    private void RemoveFromQueue()
    {
        Debug.Log("Test");
        if (CurrentState != QueueStates.InQueue) return;
        if (!_treeManager.ResearchQueue.Contains(_researchNode)) return;
        _treeManager.ResearchQueue.Remove(_researchNode);
        CurrentState= QueueStates.FinishedQueue;
    }

    public enum QueueStates { NotInQueue, InQueue, FinishedQueue }
}
