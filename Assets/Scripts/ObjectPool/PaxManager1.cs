using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaxManager1 : MonoBehaviour
{
    [SerializeField] private int _spawnAmount;
    [SerializeField] private GameObject _paxObject;
    private ObjectPool1<Pax> _pool;

    private void Awake() => _pool = new ObjectPool1<Pax>(CreateFunction, OnGetAction, OnReturnAction, _spawnAmount);

    private Pax CreateFunction()
    {
        GameObject paxObj = Instantiate(_paxObject);
        Pax pax = paxObj.GetComponent<Pax>();
        pax.SetCallback(ReturnToPool);
        return pax;
    }

    private void OnGetAction(Pax pax)
    {
        pax.gameObject.SetActive(true);
    }

    private void OnReturnAction(Pax pax)
    {
        pax.gameObject.SetActive(false);
    }

    private void ReturnToPool(Pax pax) => _pool.Return(pax);

    private void Update()
    {
        Pax pax;
        if (Input.GetKeyDown(KeyCode.K))
             pax = _pool.Get();
    }
}
