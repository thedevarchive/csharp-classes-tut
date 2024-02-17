using Classes;

public class GiftCardAccount : BankAccount
{
    private readonly decimal _monthlyDeposit = 0m;

    public GiftCardAccount(string name, decimal initialBalance, decimal monthlyDeposit = 0) : base(name, initialBalance)
        => _monthlyDeposit = monthlyDeposit;

    //this bank account adds a sum of money (monthlyDeposit) to this bank account
    //at the end of each month 
    public override void PerformMonthEndTransactions()
    {
        if (_monthlyDeposit != 0)
        {
            MakeDeposit(_monthlyDeposit, DateTime.Now, "Add monthly deposit");
        }
    }
}