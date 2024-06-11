using System;
using UnityEngine;

public class Pax : MonoBehaviour
{
    private Action<Pax> _returnCallback;

    public void SetCallback(Action<Pax> callback) => _returnCallback += callback;

    public void Return() => _returnCallback.Invoke(this);
}
