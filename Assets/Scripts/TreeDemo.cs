using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeDemo : MonoBehaviour
{
    [SerializeField] private KeyCode _showTreeButton;
    [SerializeField] private RDTreeManager _rdTreeManager;
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(_showTreeButton)) _rdTreeManager?.ShowTree();
    }
}
