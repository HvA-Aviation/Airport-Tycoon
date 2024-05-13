using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ResearchNode : MonoBehaviour
{
    [SerializeField] private RDTreeManager _treeManager;
    public float ResearchTimer { get; private set; }

    public delegate void ResearchDoneEvent();
    public ResearchDoneEvent ResearchDone { get; set; }

    public ResearchNodeSetting NodeSetting {  get; private set; }

    public SkillState CurrentSkillState { get;  set; }

    private void Start()
    {
        NodeSetting = GetComponent<ResearchNodeSetting>();

        ResearchDone += ResearchBought;
        ResearchDone += SetNextStatesInTree;
        ResearchDone += NodeSetting.UnlockObjects;
        ResearchDone += _treeManager.ResearchFinished;
    }
    
    /// <summary>
    /// This function is called when the research is done
    /// </summary>
    private void ResearchBought() => CurrentSkillState = SkillState.bought;     

    /// <summary>
    /// This function is called to set the next nodes active when the research is done
    /// </summary>
    private void SetNextStatesInTree()
    {
        foreach (var connectedNode in NodeSetting.ConnectedResearchNodes)
        {
            if (connectedNode == null) continue;

            connectedNode.CurrentSkillState = SkillState.available;
            connectedNode.GetComponent<TreeNodeDemo>().NodeStates();
        }
    }

    /// <summary>
    /// Call this function when you want the research to be developed
    /// </summary>
    /// <param name="value">The value you want to add to the time</param>
    public void AddTime(float value)
    {
        if (CurrentSkillState != SkillState.inDevelopment) return;
        ResearchTimer += value;

        if (ResearchTimer >= NodeSetting.ResearchTime)
        {
            if (ResearchDone != null)
                ResearchDone();
        }
    }

    /// <summary>
    /// This function is called to pause the research or to set nodes available
    /// </summary>
    [ContextMenu("Available")]public void SetNodeAvailable() => CurrentSkillState = SkillState.available;

    /// <summary>
    /// This function is called when you want to start the research
    /// </summary>
    public void StartResearch() 
    {
        if(CurrentSkillState != SkillState.available) return;
        _treeManager.ChooseNewResearch(this);
        CurrentSkillState = SkillState.inDevelopment; 
    }

    /// <summary>
    /// All the states from the research
    /// </summary>
    public enum SkillState { notAvailable, available, inDevelopment, bought }
}
