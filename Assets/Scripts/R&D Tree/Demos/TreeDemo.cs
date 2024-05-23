using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeDemo : MonoBehaviour
{
    [SerializeField] private KeyCode _showTreeButton;
    [SerializeField] private GameObject _tree;

    [SerializeField] private RDTreeManager _treeManager;
    // Update is called once per frame

    private void Start()
    {
        _tree.gameObject.SetActive(false);
    }

    void Update()
    {
        if(Input.GetKeyDown(_showTreeButton)) _tree.gameObject.SetActive(!_tree.activeSelf);
    }

    private void FixedUpdate() => _treeManager.CurrentResearching?.AddValue(1);
}
