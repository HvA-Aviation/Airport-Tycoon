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

    [Header("Objects for UI")]
    [SerializeField] private TMP_Text _titleText;
    [SerializeField] private TMP_Text _descriptionText;

    [SerializeField] private List<ResearchNode> _connectedResearchNodes;
    [SerializeField] private List<GameObject> _objectsToUnlock;

    public List<ResearchNode> ConnectedResearchNodes => _connectedResearchNodes;
    public int ResearchCost => _cost;
    public int ResearchTime => _researchTime;

    private void Awake()
    {
        _titleText.text = _title;
        _descriptionText.text = _description;
    }

    /// <summary>
    /// Call this function when the research is done and the objects needs to be unlocked
    /// </summary>
    public void UnlockObjects()
    {

    }
}
