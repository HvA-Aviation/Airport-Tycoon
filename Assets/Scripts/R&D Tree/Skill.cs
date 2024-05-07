using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Skill : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private Slider _researchBar;

    private float _timer;

    public UnityEvent ResearchDoneEvent = new UnityEvent();

    private ResearchNodeSetting _nodeSetting;

    public SkillState CurrentSkillState;

    private void OnEnable()
    {
        SetUIForStates();
    }

    private void Awake()
    {

    }

    private void Start()
    {
        RDTreeManager.Instance.Skills.Add(gameObject);

        _nodeSetting = GetComponent<ResearchNodeSetting>();

        _researchBar.maxValue = _nodeSetting.ResearchTime;
        AddListenersToEvent();
    }

    public void TimerForInDevelopment()
    {
        if (CurrentSkillState != SkillState.inDevelopment) return;

        _timer += Time.fixedDeltaTime;
        _researchBar.value = _timer;
        if (_timer >= _nodeSetting.ResearchTime)
        {
            ResearchDoneEvent.Invoke();
        }
    }

    private void ResearchDone()
    {
        CurrentSkillState = SkillState.bought;

        if (RDTreeManager.Instance.CurrentResearching != null)
        {
            RDTreeManager.Instance.CurrentResearching = null;
        }
    }

    public void OnClick()
    {
        if (CurrentSkillState == SkillState.available)
        {
            CurrentSkillState = SkillState.inDevelopment;
            RDTreeManager.Instance.ChooseNewResearch(this);
            SetUIForStates();
        }
    }
    public void AddListenersToEvent()
    {
        ResearchDoneEvent.AddListener(ResearchDone);
        ResearchDoneEvent.AddListener(SetNextStatesInTree);
        ResearchDoneEvent.AddListener(_nodeSetting.UnlockObjects);
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
        if (CurrentSkillState != SkillState.bought) return;

        foreach (var connectedNode in _nodeSetting.ConnectedResearchNodes)
        {
            if (connectedNode == null) continue;

            connectedNode.CurrentSkillState = SkillState.available;
            connectedNode.SetUIForStates();
        }
    }

    public void PauseResearch()
    {
        CurrentSkillState = SkillState.available;
        SetUIForStates();
    }

    public enum SkillState { notAvailable, available, inDevelopment, bought }
}
