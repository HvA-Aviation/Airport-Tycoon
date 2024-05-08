using UnityEngine;
using UnityEngine.UI;

public class ResearchNodeQueue : MonoBehaviour
{

    [SerializeField] private Button _queueButton;
    [SerializeField] private ResearchNode _skill;

    private ResearchNodeSetting _researchNodeSettings;

    public States CurrentState;

    private void Start()
    {
        _researchNodeSettings = GetComponent<ResearchNodeSetting>();

        _skill.ResearchDoneEvent.AddListener(RemoveFromQueue);
    }

    private void FixedUpdate()
    {
        if (_skill.CurrentSkillState == ResearchNode.SkillState.bought)
        {
            _queueButton.interactable = false;
            return;
        }
        NodeQueueState();
    }

    private void NodeQueueState()
    {
        switch (CurrentState)
        {
            case States.NotInQueue:
                _queueButton.interactable = true;
                break;
            case States.InQueue:
                _queueButton.interactable = false;
                break;
        }
    }

    public void OnClick()
    {
        if (CurrentState == States.InQueue) return;

        RDTreeManager.Instance.ResearchQueue.Add(_skill);
        CurrentState = States.InQueue;
    }

    private void RemoveFromQueue()
    {
        if (!RDTreeManager.Instance.ResearchQueue.Contains(_skill)) return;

        RDTreeManager.Instance.ResearchQueue.Remove(_skill);

    }

    public enum States { NotInQueue, InQueue }
}
