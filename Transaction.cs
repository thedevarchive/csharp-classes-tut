namespace Classes;

//Represents a transaction done to a user's bank account
public class Transaction
{
    //the amount deposited or withdrawn during the transaction
    public decimal Amount { get; }
    //holds when the transaction happened
    public DateTime Date { get; }
    //additional information about the transaction, if any
    public string Notes { get; }

    //constructor for Transaction
    public Transaction(decimal amount, DateTime date, string note)
    {
        Amount = amount;
        Date = date;
        Notes = note;
    }
}