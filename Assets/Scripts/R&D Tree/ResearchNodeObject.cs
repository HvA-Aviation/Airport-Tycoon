using UnityEngine;

[CreateAssetMenu(fileName = "New ResearchNode", menuName = "Research/CreateResearchNode")]
public class ResearchNodeObject : ScriptableObject
{
    [SerializeField] private string _title;
    [SerializeField] private string _description;
    [SerializeField] private int _researchPrice;
    [SerializeField] private int _researchCompletionValue;


    public string Title => _title;
    public string Description => _description;
    public int ResearchPrice => _researchPrice;
    public int ResearchCompletionValue => _researchCompletionValue;
}
