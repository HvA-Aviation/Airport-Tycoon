using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockedObjectDemo : MonoBehaviour
{
    [SerializeField] private ResearchNode _researchConnectedToObject;
    private void Start()
    {
        _researchConnectedToObject.ResearchDoneEvent.AddListener(UnlockObject);
    }
    
    private void UnlockObject() => Debug.Log("Unlock this object");
}
