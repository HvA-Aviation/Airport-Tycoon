using UnityEngine;
using UnityEngine.UI;

public class ResearchNodeQueue : MonoBehaviour
{
    [SerializeField] private ResearchNode _researchNode;
    [SerializeField] private RDTreeManager _treeManager;

    private ResearchNodeSetting _researchNodeSettings;

    public States CurrentState;

    private void Start()
    {
        _researchNodeSettings = GetComponent<ResearchNodeSetting>();

        _researchNode.ResearchDone += RemoveFromQueue;
    }  
    
    /// <summary>
    /// This function will be called when the research is done.
    /// </summary>
    private void RemoveFromQueue()
    {
        if (!_treeManager.ResearchQueue.Contains(_researchNode)) return;
        _treeManager.ResearchQueue.Remove(_researchNode);
    }

    public enum States { NotInQueue, InQueue }
}
