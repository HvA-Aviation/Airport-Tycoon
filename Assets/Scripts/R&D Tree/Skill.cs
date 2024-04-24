using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Skill : MonoBehaviour
{
    [SerializeField] private string _title;
    [SerializeField] private string _description;
    [SerializeField] private int _cost;
    [SerializeField] private int _researchTime;

    private float _timer;
    [Header("Objects for UI")]
    [SerializeField] private TMP_Text _titleText;
    [SerializeField] private TMP_Text _descriptionText;
    [SerializeField] private Image _screenImage;
    [SerializeField] private Button _button;

    [Header("Next In Tree")]
    [SerializeField] private List<Skill> ConnectedResearchNodes;

    public SkillState CurrentSkillState;

    private void OnEnable()
    {
        SetUIForStates();
    }

    private void Awake()
    {
        _titleText.text = _title;
        _descriptionText.text = _description;
        //RDTreeManager.Instance.Skills.Add(gameObject);
    }

    public void TimerForInDevelopment()
    {
        if (CurrentSkillState != SkillState.inDevelopment) return;

        _timer += Time.fixedDeltaTime;
        if(_timer >= _researchTime)
        {
            Debug.Log($"Finished research of {_title}");
            RDTreeManager.Instance.SkillsInDevelopment.Remove(this);
            CurrentSkillState = SkillState.bought;
            SetNextStatesInTree();
        }
    }

    public void OnClick()
    {
        if (CurrentSkillState == SkillState.available)
        {
            CurrentSkillState = SkillState.inDevelopment;
            RDTreeManager.Instance.SkillsInDevelopment.Add(this);
            SetUIForStates();
        }
    }

    public void SetUIForStates()
    {
        switch (CurrentSkillState)
        {
            case SkillState.notAvailable:
                _button.interactable = false;
                _screenImage.color = new Color(0, 0, 0, 1);
                break;
            case SkillState.available:
                _button.interactable = true;
                _screenImage.color = new Color(0, 0, 0, .5f);
                break;
            case SkillState.inDevelopment:
                _button.interactable = false;
                Destroy(_screenImage.gameObject);
                break;
            case SkillState.bought:
                _button.interactable = false;
                Destroy(_screenImage.gameObject);
                break;
        }
    }

    private void SetNextStatesInTree()
    {
        if(CurrentSkillState != SkillState.bought) return;

        foreach(var connectedNode in ConnectedResearchNodes)
        {
            connectedNode.CurrentSkillState = SkillState.available;
            connectedNode.SetUIForStates();
        }
    }
    public enum SkillState { notAvailable, available, inDevelopment,  bought }
}
