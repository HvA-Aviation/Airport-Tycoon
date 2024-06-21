using UnityEngine;

public class PaxManager : MonoBehaviour
{
    [SerializeField, Tooltip("The amount of new object needs to be spawned when pool is empty")]
    private int _spawnAmount;

    private int amountOfRecordedPax;
    private float totalTimeSpent;

    [SerializeField, Tooltip("The passenger gameobject")]
    private GameObject _paxObject;

    private ObjectPool<Pax> _pool;

    public ObjectPool<Pax> Pool => _pool;

    public float AverageTimeInAirport => amountOfRecordedPax == 0 ? 0 : totalTimeSpent / amountOfRecordedPax;
    public int AmountOfPaxInAirport => _pool.GetAmountOfActivePax();

    private void Awake() => _pool = new ObjectPool<Pax>(CreateFunction, OnGetAction, OnReturnAction, _spawnAmount);

    /// <summary>
    /// This function is called when there are not enough objects in the pool so it will create new objects
    /// </summary>
    /// <returns>The pax class that needs to be on the passenger object</returns>
    private Pax CreateFunction()
    {
        GameObject paxObj = Instantiate(_paxObject);
        Pax pax = paxObj.GetComponent<Pax>();
        pax.SetCallback(ReturnToPool);
        return pax;
    }

    /// <summary>
    /// Function that is called when you get the object out of the pool
    /// </summary>
    /// <param name="pax">The pax script that is of that object</param>
    private void OnGetAction(Pax pax)
    {
        pax.gameObject.SetActive(true);
    }

    /// <summary>
    /// Function that is called when returning the object to the pool
    /// </summary>
    /// <param name="pax">The pax script that is of that object</param>
    private void OnReturnAction(Pax pax)
    {
        pax.gameObject.SetActive(false);
    }

    /// <summary>
    /// Function that needs to be called by the pax script when it wants to return to the pool
    /// </summary>
    /// <param name="pax">The pax script it needs to return</param>
    private void ReturnToPool(Pax pax)
    {
        amountOfRecordedPax++;
        totalTimeSpent += pax.timeElapsed;
        _pool.Return(pax);
    }
}
