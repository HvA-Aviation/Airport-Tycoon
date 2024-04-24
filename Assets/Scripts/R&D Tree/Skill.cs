using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
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
    [SerializeField] private Button _button;

    [Header("Next In Tree")]
    [SerializeField] private List<Skill> ConnectedResearchNodes;

    [SerializeField] private List<GameObject> ObjectToUnlock;

    public SkillState CurrentSkillState;

    private void OnEnable()
    {
        SetUIForStates();
    }

    private void Awake()
    {
        _titleText.text = _title;
        _descriptionText.text = _description;
    }

    private void Start()
    {
        RDTreeManager.Instance.Skills.Add(gameObject);
    }

    public void TimerForInDevelopment()
    {
        if (CurrentSkillState != SkillState.inDevelopment) return;

        _timer += Time.fixedDeltaTime;
        if(_timer >= _researchTime)
        {
            RDTreeManager.Instance.ResearchDoneEvent.Invoke();
            RDTreeManager.Instance.ResearchDoneEvent = new UnityEvent();
        }
    }

    private void ResearchDone()
    {
        Debug.Log($"Finished Research for {_title}");
        RDTreeManager.Instance.SkillsInDevelopment.Remove(this);
        CurrentSkillState = SkillState.bought;
    }

    private void UnlockObjects()
    {
        Debug.Log($"Unlocked projects {ObjectToUnlock.Count}");
    }

    public void OnClick()
    {
        if (CurrentSkillState == SkillState.available)
        {
            CurrentSkillState = SkillState.inDevelopment;
            RDTreeManager.Instance.SkillsInDevelopment.Add(this);
            SetUIForStates();

            RDTreeManager.Instance.ResearchDoneEvent.AddListener(ResearchDone);
            RDTreeManager.Instance.ResearchDoneEvent.AddListener(SetNextStatesInTree);
            RDTreeManager.Instance.ResearchDoneEvent.AddListener(UnlockObjects);
        }
    }

    public void SetUIForStates()
    {
        switch (CurrentSkillState)
        {
            case SkillState.notAvailable:
                _button.interactable = false;
                gameObject.SetActive(false);
                break;
            case SkillState.available:
                _button.interactable = true;
                gameObject.SetActive(true);
                break;
            case SkillState.inDevelopment:
                _button.interactable = false;
                break;
            case SkillState.bought:
                _button.interactable = false;
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
