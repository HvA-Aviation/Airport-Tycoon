using Features.Managers;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameTimeUIDemo : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;

    public void UpdateText()
    {
        _text.text = "Speed: " + GameManager.Instance.GameTimeManager.TimeScale;
    }

}
