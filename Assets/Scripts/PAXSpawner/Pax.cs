using System;
using Features.Managers;
using UnityEngine;

public class Pax : MonoBehaviour
{
    [HideInInspector] public float timeElapsed;
    private Action<Pax> _returnCallback;

    void OnEnable() => timeElapsed = 0;

    void FixedUpdate() => timeElapsed += GameManager.Instance.GameTimeManager.DeltaTime;

    public void SetCallback(Action<Pax> callback) => _returnCallback += callback;

    public void Return() => _returnCallback.Invoke(this);
}
