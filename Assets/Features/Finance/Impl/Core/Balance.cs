using Features.EventManager;
using Features.Managers;

public class Balance
{
    public int Value {get; private set;}

    public void Add(int amount) 
    {
        Value += amount;
        GameManager.Instance.EventManager.TriggerEvent(EventId.OnMoneyAdded);
    }
    
    public void Subtract(int amount) 
    {
        Value -= amount;
        GameManager.Instance.EventManager.TriggerEvent(EventId.OnMoneyRemoved);
    }
}
