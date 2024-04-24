using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RDTreeManager : MonoBehaviour
{
    [SerializeField] private GameObject _skillTree;

    private RDControls _controls;

    public static RDTreeManager Instance;

    public List<GameObject> Skills { get; private set; }

    public List<Skill> SkillsInDevelopment = new List<Skill>();

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
        if(Instance == null) Instance = this;
        else Destroy(Instance);

        _controls = new RDControls();
        _controls.Enable();
    }

    private void FixedUpdate()
    {
        foreach (var skill in SkillsInDevelopment)
            skill.TimerForInDevelopment();
    }

    private void ShowTree(InputAction.CallbackContext _) => _skillTree.SetActive(!_skillTree.activeSelf);

}
