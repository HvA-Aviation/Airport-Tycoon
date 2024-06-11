using System.Collections.Generic;
using System.Linq;

public class LoanManager<T> where T : LoanBase
{
    private List<Loan<T>> _listOfLoans = new List<Loan<T>>();

    public LoanManager(List<T> listOfLoans)
    {
        foreach (T loan in listOfLoans)
            _listOfLoans.Add(new Loan<T>(loan));
    }

    public Loan<T> GetLoan(int index) => _listOfLoans[index];

    public float AcceptLoan(int index) => _listOfLoans[index].Accept(); 


    public float AdvancePeriod()
    {
        List<Loan<T>> finishedLoans = new List<Loan<T>>();
        
        // Check if loan is done
        List<Loan<T>> activeLoans = _listOfLoans.Where(l => l.State == LoanState.InProcess).ToList();
        foreach (Loan<T> loan in activeLoans)
            if(loan.AdvancePeriod()) 
                finishedLoans.Add(loan);
            
        // Calculate period payment
        float payment = 0;
        foreach (Loan<T> loan in activeLoans)
            payment += loan.GetOwed();

        foreach (Loan<T> loan in finishedLoans)
            loan.Finish();

        return payment;
    }
}