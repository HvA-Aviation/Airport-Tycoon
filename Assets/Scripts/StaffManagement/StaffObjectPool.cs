using Features.Managers;
using Features.Workers;
using UnityEngine;

public class StaffObjectPool : MonoBehaviour
{
    [SerializeField, Tooltip("The staff gameobject")]
    private GameObject _staffObject;

    [SerializeField, Tooltip("The amount of new object needs to be spawned when pool is empty")]
    private int _spawnAmount;
    public ObjectPool<Employee> EmployeeObjectPool { get; private set; }

    private void Awake() => EmployeeObjectPool = new ObjectPool<Employee>(CreateFunction, OnGetAction, OnReturnAction, _spawnAmount);

    private void Start()
    {
        var employee = EmployeeObjectPool.Get();
        employee.Return();
    }

    /// <summary>
    /// This function is called when there are not enough objects in the pool so it will create new objects
    /// </summary>
    /// <returns>The employee class that needs to be on the passenger object</returns>
    private Employee CreateFunction()
    {
        GameObject staffObj = Instantiate(_staffObject, transform);
        Employee employee = staffObj.GetComponent<Employee>();
        employee.SetCallback(ReturnToPool);

        SubscribeToMultiplierAction(employee.EmployeeType, staffObj);

        return employee;
    }

    /// <summary>
    /// Function that is called when you get the object out of the pool
    /// </summary>
    /// <param name="employee">The employee script that is of that object</param>
    private void OnGetAction(Employee employee)
    {
        employee.gameObject.SetActive(true);
    }

    /// <summary>
    /// Function that is called when returning the object to the pool
    /// </summary>
    /// <param name="employee">The employee script that is of that object</param>
    public void OnReturnAction(Employee employee)
    {
        employee.gameObject.SetActive(false);
    }

    /// <summary>
    /// Function that needs to be called by the employee script when it wants to return to the pool
    /// </summary>
    /// <param name="employee">The employee script it needs to return</param>
    private void ReturnToPool(Employee employee) => EmployeeObjectPool.Return(employee);

    private void SubscribeToMultiplierAction(Employee.EmployeeTypes type, GameObject staffOBJ)
    {
        if (staffOBJ == null)
            return;

        if (type == Employee.EmployeeTypes.Builder)
            GameManager.Instance.UpgradeManager.SetMultiplierBuilder += staffOBJ.GetComponent<BuilderWorker>().SetWorkSpeed;
        else if (type == Employee.EmployeeTypes.Security)
            GameManager.Instance.UpgradeManager.SetMultiplierSecurity += staffOBJ.GetComponent<SecurityWorker>().SetWorkSpeed;
        else if (type == Employee.EmployeeTypes.Staff)
            GameManager.Instance.UpgradeManager.SetMultiplierGeneralWorker += staffOBJ.GetComponent<GeneralWorker>().SetWorkSpeed;

    }
}
