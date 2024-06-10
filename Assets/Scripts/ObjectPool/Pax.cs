using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Pax : MonoBehaviour
{
    private Action<Pax> _returnCallback;

    public void SetCallback(Action<Pax> callback) => _returnCallback = callback;

    public void Return() => _returnCallback.Invoke(this);

    private void OnEnable()
    {
        Invoke("Return", 10f);
    }
}
