using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadSceneASync : MonoBehaviour
{
    [Header("Menu Objects")]
    [SerializeField] private GameObject _menuObj;
    [SerializeField] private GameObject _loadingScreenObj;

    [SerializeField] private Slider _loadingBarSlider;

    public void LoadLevel(int sceneIndex)
    {
        StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    private IEnumerator LoadAsynchronously(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        _menuObj.SetActive(false);
        _loadingScreenObj.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f );
            _loadingBarSlider.value = progress;

            yield return null;
        }
    }
}
