using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class RDTreeManager : MonoBehaviour
{
    [SerializeField] private GameObject _skillTree;

    private RDControls _controls;

    public static RDTreeManager Instance;

    public List<GameObject> Skills = new List<GameObject>();

    public Skill CurrentResearching;

    public List<Skill> ResearchQueue = new List<Skill>();

    public List<Skill> ResearchedDone = new List<Skill>();

    private void OnEnable()
    {
        _controls.RDControlls.ShowTree.performed += ShowTree;
    }

    private void OnDisable()
    {
        _controls.RDControlls.ShowTree.performed -= ShowTree;
    }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(Instance);

        _controls = new RDControls();
        _controls.Enable();
    }

    private void FixedUpdate()
    {
        if (ResearchQueue.Count > 0 && CurrentResearching == null)
        {
            CurrentResearching = ResearchQueue[0];
            CurrentResearching.GetComponent<ResearchNodeQueue>().CurrentState = ResearchNodeQueue.States.InQueue;
            ResearchQueue.RemoveAt(0);
            CurrentResearching.CurrentSkillState = Skill.SkillState.inDevelopment;
        }

        Debug.Log(CurrentResearching);
        CurrentResearching?.TimerForInDevelopment();
    }

    private void ShowTree(InputAction.CallbackContext _) => _skillTree.SetActive(!_skillTree.activeSelf);

    public void ChooseNewResearch(Skill newResearch)
    {
        if (CurrentResearching != null)
            CurrentResearching.PauseResearch();

        CurrentResearching = newResearch;
    }
}
