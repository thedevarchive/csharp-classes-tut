namespace Classes;

//Represents a user's bank account and the amount of money it holds
public class BankAccount
{
    private static int s_accountNumberSeed = 1234567890;

    private readonly decimal _minimumBalance;

    // account number (serves as unique identifier)
    public string Number { get; }
    // name of the owner
    public string Owner { get; set; }
    //the amount of money currently in the account
    public decimal Balance
    {
        get
        {
            decimal balance = 0;
            foreach (var item in _allTransactions)
            {
                balance += item.Amount;
            }

            return balance;
        }
    }

    //constructors for BankAccount
    //constructor for accounts with no minimum balance
    public BankAccount(string name, decimal initialBalance) : this(name, initialBalance, 0) { }

    //constructor for accounts with a minimum balance
    //needed especially for LineOfCreditAccount because it allows users to withdraw past 0 balance
    public BankAccount(string name, decimal initialBalance, decimal minimumBalance)
    {
        Number = s_accountNumberSeed.ToString();
        s_accountNumberSeed++;

        Owner = name;
        _minimumBalance = minimumBalance;
        if (initialBalance > 0)
            MakeDeposit(initialBalance, DateTime.Now, "Initial balance");
    }

    //records all the transactions made on the BankAccount
    private List<Transaction> _allTransactions = new List<Transaction>();

    //throws an exception if value is negative
    //otherwise, adds amount to balance and the transaction will be recorded
    public void MakeDeposit(decimal amount, DateTime date, string note)
    {
        if (amount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "Amount of deposit must be positive");
        }
        var deposit = new Transaction(amount, date, note);
        _allTransactions.Add(deposit);
    }

    //throws an exception if value is negative
    //then, call CheckWithdrawalLimit to see if account will be overdrawn after the transaction
    //if successful, deducts amount to balance and the transaction will be recorded
    public void MakeWithdrawal(decimal amount, DateTime date, string note)
    {
        if (amount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "Amount of withdrawal must be positive");
        }
        Transaction? overdraftTransaction = CheckWithdrawalLimit(Balance - amount < _minimumBalance);
        Transaction? withdrawal = new(-amount, date, note);
        _allTransactions.Add(withdrawal);
        if (overdraftTransaction != null)
            _allTransactions.Add(overdraftTransaction);
    }

    //checks if account will go past minimumBalance
    protected virtual Transaction? CheckWithdrawalLimit(bool isOverdrawn)
    {
        if (isOverdrawn)
        {
            throw new InvalidOperationException("Not sufficient funds for this withdrawal");
        }
        else
        {
            return default;
        }
    }

    //returns a string that shows an account's list of transactions
    public string GetAccountHistory()
    {
        var report = new System.Text.StringBuilder();

        decimal balance = 0;
        report.AppendLine("Date\t\tAmount\tBalance\tNote");
        foreach (var item in _allTransactions)
        {
            balance += item.Amount;
            report.AppendLine($"{item.Date.ToShortDateString()}\t{item.Amount}\t{balance}\t{item.Notes}");
        }

        return report.ToString();
    }

    public virtual void PerformMonthEndTransactions() { }
}