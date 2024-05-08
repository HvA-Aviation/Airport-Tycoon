using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RDTreeManager : MonoBehaviour
{
    [SerializeField] private GameObject _skillTree;

    private RDControls _controls;

    public static RDTreeManager Instance;


    public List<GameObject> Skills = new List<GameObject>();



    public ResearchNode CurrentResearching;

    public List<ResearchNode> ResearchQueue = new List<ResearchNode>();

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
            CurrentResearching.CurrentSkillState = ResearchNode.SkillState.inDevelopment;
        }

        CurrentResearching?.TimerForInDevelopment();
    }

    public void ChooseNewResearch(ResearchNode newResearch)
    {
        if (CurrentResearching != null)
            CurrentResearching.PauseResearch();

        CurrentResearching = newResearch;
    }

    private void ShowTree(InputAction.CallbackContext _) => _skillTree.SetActive(!_skillTree.activeSelf);
}
