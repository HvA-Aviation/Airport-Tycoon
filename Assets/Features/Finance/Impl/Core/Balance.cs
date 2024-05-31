public class Balance
{
    public int Value {get; private set;}

    public void Add(int amount) 
    {
        Value += amount;
    }
    
    public void Subtract(int amount) 
    {
        Value -= amount;
    }
}
