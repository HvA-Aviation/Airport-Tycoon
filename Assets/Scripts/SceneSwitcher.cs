using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    [SerializeField] private string _sceneName;
    [SerializeField] private int _sceneIndex;

    public void SwitchScene()
    {
         if(_sceneName != "" && _sceneName != null)
            SceneManager.LoadScene(_sceneName);
         else
            SceneManager.LoadScene(_sceneIndex);
    }
}
