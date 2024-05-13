using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeDemo : MonoBehaviour
{
    [SerializeField] private KeyCode _showTreeButton;
    [SerializeField] private GameObject _tree;
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(_showTreeButton)) _tree.gameObject.SetActive(!_tree.activeSelf);
    }
}
