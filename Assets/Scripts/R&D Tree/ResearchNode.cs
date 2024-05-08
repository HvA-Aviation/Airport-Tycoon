using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ResearchNode : MonoBehaviour
{
    public float ResearchTimer { get; private set; }

    public delegate void ResearchDoneEvent();

    public ResearchDoneEvent ResearchDone { get; set; }

    private ResearchNodeSetting _nodeSetting;

    public SkillState CurrentSkillState;

    private void Start()
    {
        _nodeSetting = GetComponent<ResearchNodeSetting>();

        ResearchDone += ResearchBought;
        ResearchDone += SetNextStatesInTree;
        ResearchDone += _nodeSetting.UnlockObjects;
    }

    private void FixedUpdate() => CheckIfResearchIsDone();
    
    /// <summary>
    /// This function is called when the research is done
    /// </summary>
    private void ResearchBought()
    {
        CurrentSkillState = SkillState.bought;        
    }

    /// <summary>
    /// Call this function in the fixedUpdate to check if the research is done
    /// </summary>
    private void CheckIfResearchIsDone()
    {
        if(ResearchTimer >= _nodeSetting.ResearchTime)
        {
            ResearchDone?.Invoke();
        }
    }

    /// <summary>
    /// This function is called to set the next nodes active when the research is done
    /// </summary>
    private void SetNextStatesInTree()
    {
        if (CurrentSkillState != SkillState.bought) return;

        foreach (var connectedNode in _nodeSetting.ConnectedResearchNodes)
        {
            if (connectedNode == null) continue;

            connectedNode.CurrentSkillState = SkillState.available;
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
    }

    /// <summary>
    /// This function is called to pause the research
    /// </summary>
    public void PauseResearch() => CurrentSkillState = SkillState.available;    

    public enum SkillState { notAvailable, available, inDevelopment, bought }
}
