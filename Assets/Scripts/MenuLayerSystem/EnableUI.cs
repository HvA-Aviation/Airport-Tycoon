using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableUI : MonoBehaviour
{
    [SerializeField] private KeyCode _dissableEnableScreen;
    [SerializeField] private GameObject _menuGFX;

    private void Update()
    {
        if (Input.GetKeyDown(_dissableEnableScreen))
            _menuGFX.SetActive(!_menuGFX.activeSelf);
    }
}
