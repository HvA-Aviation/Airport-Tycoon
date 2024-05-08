using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuDemoScript : MonoBehaviour
{
    [SerializeField] private KeyCode _enableScreen;
    [SerializeField] private KeyCode _dissableScreen;

    [SerializeField] private EnableUI _enableUI;
    [SerializeField] private DissableUI _dissableUI;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(_enableScreen))
            _enableUI.PressEnableUIButton.Invoke();

        if(Input.GetKeyDown(_dissableScreen))
            _dissableUI.PressDissableUIButton.Invoke(); 
    }
}
