using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ResearchNode : MonoBehaviour
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

    private void Start()
    {
        _nodeSetting = GetComponent<ResearchNodeSetting>();

        _researchBar.maxValue = _nodeSetting.ResearchTime;
        AddListenersToEvent();
    }

    /// <summary>
    /// This function is called when the researchnode started to be researched
    /// </summary>
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

    /// <summary>
    /// This function is called when the research is done
    /// </summary>
    private void ResearchDone()
    {
        CurrentSkillState = SkillState.bought;

        if (RDTreeManager.Instance.CurrentResearching != null)
        {
            RDTreeManager.Instance.CurrentResearching = null;
        }
    }

    /// <summary>
    /// This function is called when the start research button is clicked
    /// </summary>
    public void OnClick()
    {
        if (CurrentSkillState == SkillState.available)
        {
            CurrentSkillState = SkillState.inDevelopment;
            RDTreeManager.Instance.ChooseNewResearch(this);
            SetUIForStates();
        }
    }

    /// <summary>
    /// This function is called in the start to add all the listeners to the UnityEvent
    /// </summary>
    public void AddListenersToEvent()
    {
        ResearchDoneEvent.AddListener(ResearchDone);
        ResearchDoneEvent.AddListener(SetNextStatesInTree);
        ResearchDoneEvent.AddListener(_nodeSetting.UnlockObjects);
    }

    /// <summary>
    /// This function is called to update the UI for the node
    /// </summary>
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
            connectedNode.SetUIForStates();
        }
    }

    /// <summary>
    /// This function is called to pause the research
    /// </summary>
    public void PauseResearch()
    {
        CurrentSkillState = SkillState.available;
        SetUIForStates();
    }

    public enum SkillState { notAvailable, available, inDevelopment, bought }
}
