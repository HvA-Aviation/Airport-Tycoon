using System.Collections.Generic;
using UnityEngine;

public class ShowStaffInList : MonoBehaviour
{
    [SerializeField] private GameObject _content;

    [SerializeField] private GameObject _staffButton;
    public void AddItemToList()
    {
        Instantiate(_staffButton, _content.transform);   
    }
}
