using UnityEngine;
using UnityEngine.UI;

public class ResearchNodeQueue : MonoBehaviour
{
    [SerializeField] private ResearchNode _researchNode;
    [SerializeField] private RDTreeManager _treeManager;

    public ResearchNode ThisResearchNode =>_researchNode;
    public QueueStates CurrentState { get; set; }

    private void Start()
    {
        _researchNode.ResearchDone += RemoveFromQueue;
    }
    public void AddNodeToQueue() => _treeManager.AddResearchToQueue(this);        
    private void RemoveFromQueue() => _treeManager.RemoveResearchFromQueue(this);
    public enum QueueStates { NotInQueue, InQueue, FinishedQueue }
}
