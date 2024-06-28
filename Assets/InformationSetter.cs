using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InformationSetter : MonoBehaviour
{
    [SerializeField] TMP_Text HeaderText;
    [SerializeField] TMP_Text ValueText;

    public void SetInformation(string headerText, string valueText)
    {
        HeaderText.text = headerText;
        ValueText.text = valueText;
    }
}
