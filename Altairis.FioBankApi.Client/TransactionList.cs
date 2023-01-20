namespace Altairis.FioBankApi.Client;

public class TransactionList {

    /// <summary>
    /// Gets or sets the account number.
    /// </summary>
    /// <value>
    /// The account number.
    /// </value>
    public string AccountId { get; internal set; } = string.Empty;

    /// <summary>
    /// Gets or sets the bank code.
    /// </summary>
    /// <value>
    /// The bank code.
    /// </value>
    public string BankId { get; internal set; } = string.Empty;

    /// <summary>
    /// Gets or sets the currency code (ie. CZK).
    /// </summary>
    /// <value>
    /// The currency code.
    /// </value>
    public string Currency { get; internal set; } = string.Empty;

    /// <summary>
    /// Gets or sets the IBAN.
    /// </summary>
    /// <value>
    /// The IBAN.
    /// </value>
    public string Iban { get; internal set; } = string.Empty;

    /// <summary>
    /// Gets or sets the BIC.
    /// </summary>
    /// <value>
    /// The BIC.
    /// </value>
    public string Bic { get; internal set; } = string.Empty;

    /// <summary>
    /// Gets or sets the first transaction identifier in this list.
    /// </summary>
    /// <value>
    /// The first transaction identifier.
    /// </value>
    public string IdFrom { get; internal set; } = string.Empty;

    /// <summary>
    /// Gets or sets the last transaction identifier in this list.
    /// </summary>
    /// <value>
    /// The last transaction identifier.
    /// </value>
    public string IdTo { get; internal set; } = string.Empty;

    /// <summary>
    /// Gets or sets the account balance at beginning of this list.
    /// </summary>
    /// <value>
    /// The opening balance.
    /// </value>
    public decimal OpeningBalance { get; internal set; }

    /// <summary>
    /// Gets or sets the account balance at end of this list.
    /// </summary>
    /// <value>
    /// The closing balance.
    /// </value>
    public decimal ClosingBalance { get; internal set; }

    /// <summary>
    /// Gets or sets the starting date for this list.
    /// </summary>
    /// <value>
    /// The starting date.
    /// </value>
    public DateOnly DateStart { get; internal set; }

    /// <summary>
    /// Gets or sets the ending date for this list.
    /// </summary>
    /// <value>
    /// The ending date.
    /// </value>
    public DateOnly DateEnd { get; internal set; }

    /// <summary>
    /// Gets or sets the transactions.
    /// </summary>
    /// <value>
    /// The transactions.
    /// </value>
    public ICollection<TransactionInfo> Items { get; internal set; } = new List<TransactionInfo>();

}

public record struct TransactionInfo(
    string Id,
    DateOnly Date,
    decimal Amount,
    string Currency,
    string OtherAccountNumber,
    string OtherAccountName,
    string OtherAccountBankCode,
    string OtherAccountBankName,
    string KS,
    string VS,
    string SS,
    string UserIdentification,
    string MessageForRecipient,
    string Type,
    string Person,
    string Details,
    string Comments,
    string BIC,
    string OrderId);