using Features.EventManager;
using Features.Managers;

public class Finance
{
    public int Balance {get; private set;}

    public void Add(int amount) 
    { 
        Balance += amount;
        GameManager.Instance.EventManager.TriggerEvent(EventId.OnMoneyAdded, new BalanceEventArgs(amount));
    }
    
    public void Remove(int amount) 
    { 
        Balance -= amount;
        GameManager.Instance.EventManager.TriggerEvent(EventId.OnMoneyRemoved, new BalanceEventArgs(amount));
    }
}
