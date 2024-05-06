using System.Collections;
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

    public int ResearchCost => _cost;
    public int ResearchTime => _researchTime;

    private void Awake()
    {
        _titleText.text = _title;
        _descriptionText.text = _description;
    }
}
