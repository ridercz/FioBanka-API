namespace Altairis.FioBankApi.Client;

public class TransactionList {

    /// <summary>
    /// Gets the account number.
    /// </summary>
    /// <value>
    /// The account number.
    /// </value>
    public string AccountId { get; internal set; } = string.Empty;

    /// <summary>
    /// Gets the bank code.
    /// </summary>
    /// <value>
    /// The bank code.
    /// </value>
    public string BankId { get; internal set; } = string.Empty;

    /// <summary>
    /// Gets the currency code (ie. CZK).
    /// </summary>
    /// <value>
    /// The currency code.
    /// </value>
    public string Currency { get; internal set; } = string.Empty;

    /// <summary>
    /// Gets the IBAN.
    /// </summary>
    /// <value>
    /// The IBAN.
    /// </value>
    public string Iban { get; internal set; } = string.Empty;

    /// <summary>
    /// Gets the BIC.
    /// </summary>
    /// <value>
    /// The BIC.
    /// </value>
    public string Bic { get; internal set; } = string.Empty;

    /// <summary>
    /// Gets the first transaction identifier in this list.
    /// </summary>
    /// <value>
    /// The first transaction identifier.
    /// </value>
    public string IdFrom { get; internal set; } = string.Empty;

    /// <summary>
    /// Gets the last transaction identifier in this list.
    /// </summary>
    /// <value>
    /// The last transaction identifier.
    /// </value>
    public string IdTo { get; internal set; } = string.Empty;

    /// <summary>
    /// Gets the account balance at beginning of this list.
    /// </summary>
    /// <value>
    /// The opening balance.
    /// </value>
    public decimal OpeningBalance { get; internal set; }

    /// <summary>
    /// Gets the account balance at end of this list.
    /// </summary>
    /// <value>
    /// The closing balance.
    /// </value>
    public decimal ClosingBalance { get; internal set; }

    /// <summary>
    /// Gets the starting date for this list.
    /// </summary>
    /// <value>
    /// The starting date.
    /// </value>
    public DateOnly DateStart { get; internal set; }

    /// <summary>
    /// Gets the ending date for this list.
    /// </summary>
    /// <value>
    /// The ending date.
    /// </value>
    public DateOnly DateEnd { get; internal set; }

    /// <summary>
    /// Gets the ID of last downloaded transaction ID.
    /// </summary>
    /// <value>
    /// The ID of last downloaded transaction ID.
    /// </value>
    public string? IdLastDownload { get; internal set; }

    /// <summary>
    /// Gets the list of transactions.
    /// </summary>
    /// <value>
    /// The list of transactions.
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