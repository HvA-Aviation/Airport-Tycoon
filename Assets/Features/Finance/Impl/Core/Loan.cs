public class Loan<T> where T : LoanBase
{
    public LoanState State { get; private set; }
    public int Amount { get; private set; }
    public float Interest {  get; private set; }
    public int PaymentPeriod {  get; private set; }
    public int PeriodProgression { get; private set; }
    public T Information { get; private set; }

    public Loan(T loan)
    {
        Information = loan;
        Amount = loan.Amount;
        Interest = (100 + loan.Interest) / 100f;
        PaymentPeriod = loan.PaymentPeriod;
        Reset();
    }

    public float Accept()
    {
        State = LoanState.InProcess;
        return Amount;
    }

    public void Finish()
    {
        Reset();
    }

    public bool AdvancePeriod()
    {
        PeriodProgression++;
        return PeriodProgression == PaymentPeriod;
    }

    public float GetOwed() => Amount / PaymentPeriod * Interest;

    private void Reset()
    {
        PeriodProgression = 0;
        State = LoanState.Idle;
    }
}