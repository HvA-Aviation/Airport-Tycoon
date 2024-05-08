using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResearchNodeSetting : MonoBehaviour
{
    [SerializeField] private string _title;
    [SerializeField] private string _description;
    [SerializeField] private int _cost;
    [SerializeField] private int _researchTime;

    [SerializeField] private List<ResearchNode> _connectedResearchNodes;
    [SerializeField] private List<GameObject> _objectsToUnlock;

    public string Title => _title;
    public string Description => _description;
    public int ResearchCost => _cost;
    public int ResearchTime => _researchTime;
    public List<ResearchNode> ConnectedResearchNodes => _connectedResearchNodes;

    /// <summary>
    /// Call this function when the research is done and the objects needs to be unlocked
    /// </summary>
    public void UnlockObjects()
    {

    }
}
