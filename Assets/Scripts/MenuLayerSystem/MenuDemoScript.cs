using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuDemoScript : MonoBehaviour
{
    [SerializeField] private KeyCode _dissableEnableScreen;

    [SerializeField] private EnableUI _enableUI;   

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(_dissableEnableScreen))
            _enableUI.PressEnableUIButton.Invoke();
    }
}
