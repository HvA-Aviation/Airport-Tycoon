using System;

public class BalanceEventArgs : EventArgs
{
    public int Amount {get; private set;}

    public BalanceEventArgs(int amount) { Amount = amount; }

}