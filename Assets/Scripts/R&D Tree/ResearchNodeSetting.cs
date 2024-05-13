using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResearchNodeSetting : MonoBehaviour
{
    [SerializeField] private string _title;
    [SerializeField] private string _description;
    [SerializeField] private int _researchPrice;
    [SerializeField] private int _researchCompletionValue;

    [SerializeField] private List<ResearchNode> _connectedResearchNodes;

    public string Title => _title;
    public string Description => _description;
    public int ResearchPrice => _researchPrice;
    public int ResearchCompletionValue => _researchCompletionValue;
    public List<ResearchNode> ConnectedResearchNodes => _connectedResearchNodes;    
}
