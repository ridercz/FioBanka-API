using CsvHelper;
using CsvHelper.Configuration;

namespace Altairis.FioBankApi.Client;

public class FioClient : IDisposable {
    private readonly HttpClient httpClient;

    // Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="FioClient"/> class.
    /// </summary>
    /// <param name="token">The access token.</param>
    public FioClient(string token) {
        this.Token = token;

        // Setup HTTP client
        var appVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
        this.httpClient = new HttpClient();
        this.httpClient.DefaultRequestHeaders.UserAgent.Clear();
        this.httpClient.DefaultRequestHeaders.UserAgent.Add(new System.Net.Http.Headers.ProductInfoHeaderValue("Altairis.FioBankApi.Client", appVersion?.ToString()));
    }

    // Configuration properties

    /// <summary>
    /// Gets the access token.
    /// </summary>
    /// <value>
    /// The access token.
    /// </value>
    public string Token { get; init; }

    // Public API

    /// <summary>
    /// Gets list of transactions from last cursor position.
    /// </summary>
    /// <returns></returns>
    public Task<TransactionList> GetTransactions()
        => this.GetTransactions($"https://www.fio.cz/ib_api/rest/last/{this.Token}/transactions.csv");

    /// <summary>
    /// Gets list of transactions from the specified date until today.
    /// </summary>
    /// <param name="dateBegin">The date range begin.</param>
    /// <returns></returns>
    public Task<TransactionList> GetTransactions(DateOnly dateBegin)
        => this.GetTransactions(dateBegin, DateOnly.FromDateTime(DateTime.Today));

    /// <summary>
    /// Gets list of transactions in the specified date range.
    /// </summary>
    /// <param name="dateBegin">The date range begin.</param>
    /// <param name="dateEnd">The date range end.</param>
    /// <returns></returns>
    public Task<TransactionList> GetTransactions(DateOnly dateBegin, DateOnly dateEnd)
        => this.GetTransactions($"https://www.fio.cz/ib_api/rest/periods/{this.Token}/{dateBegin:yyyy-MM-dd}/{dateEnd:yyyy-MM-dd}/transactions.csv");

    /// <summary>
    /// Sets the cursor to last known transaction ID.
    /// </summary>
    /// <param name="lastKnownId">The last known transaction ID.</param>
    /// <returns></returns>
    public Task SetCursor(string lastKnownId)
        => this.GetHttpStream($"https://www.fio.cz/ib_api/rest/set-last-id/{this.Token}/{lastKnownId}/");

    /// <summary>
    /// Sets the cursor to last failed date.
    /// </summary>
    /// <param name="lastFailedDate">The last failed date.</param>
    /// <returns></returns>
    public Task SetCursor(DateOnly lastFailedDate)
        => this.GetHttpStream($"https://www.fio.cz/ib_api/rest/set-last-date/{this.Token}/{lastFailedDate:yyyy-MM-dd}/");
    
    // IDisposable implementation
    
    public void Dispose() => ((IDisposable)this.httpClient).Dispose();

    // Helper methods

    private async Task<Stream> GetHttpStream(string url) {
        var rp = await this.httpClient.GetAsync(url);
        rp.EnsureSuccessStatusCode();
        return await rp.Content.ReadAsStreamAsync();
    }

    private async Task<TransactionList> GetTransactions(string url) {
        // Api uses the cs-CZ culture for decimal separators and date format
        var czechCulture = System.Globalization.CultureInfo.GetCultureInfo("cs-CZ");

        // Prepare result object
        var result = new TransactionList();

        // Get stream from API
        var csvStream = await this.GetHttpStream(url);
        var csvTextReader = new StreamReader(csvStream, encoding: System.Text.Encoding.UTF8, leaveOpen: true);

        // Read header
        var numberOfHeaderRows = 0;
        while (true) {
            var line = await csvTextReader.ReadLineAsync();
            numberOfHeaderRows++;

            // Read until empty line
            if (string.IsNullOrEmpty(line)) break;
            var data = line.Split(';');
            if (data.Length < 2) {
                // Too few columns; this is unexpected but not fatal, just ignore it.
                continue;
            } else if (data.Length > 2) {
                // Too many columns for a header; most likely first row of CSV, containing the column names. See HACK comment below.
                numberOfHeaderRows--;
                break;
            }

            // Parse key:value pair
            switch (data[0]) {
                case "accountId":
                    result.AccountId = data[1];
                    break;
                case "bankId":
                    result.BankId = data[1];
                    break;
                case "currency":
                    result.Currency = data[1];
                    break;
                case "iban":
                    result.Iban = data[1];
                    break;
                case "bic":
                    result.Bic = data[1];
                    break;
                case "idFrom":
                    result.IdFrom = data[1];
                    break;
                case "idTo":
                    result.IdTo = data[1];
                    break;
                case "idLastDownload":
                    result.IdLastDownload = data[1];
                    break;
                case "openingBalance":
                    result.OpeningBalance = decimal.Parse(data[1], czechCulture);
                    break;
                case "closingBalance":
                    result.ClosingBalance = decimal.Parse(data[1], czechCulture);
                    break;
                case "dateStart":
                    result.DateStart = DateOnly.ParseExact(data[1], "dd.MM.yyyy");
                    break;
                case "dateEnd":
                    result.DateEnd = DateOnly.ParseExact(data[1], "dd.MM.yyyy");
                    break;
                default:
                    break;
            }
        }

        // Reset the reader
        // HACK: The weird thing with counting the header rows and then reopening the reader and ignoring them is that
        //       due to a bug in FIO Bank API, which does not insert the empty line after headers when calling the
        //       `/ib_api/rest/last/` endpoint.
        csvTextReader.Close();
        csvStream.Seek(0, SeekOrigin.Begin);
        csvTextReader = new StreamReader(csvStream, encoding: System.Text.Encoding.UTF8, leaveOpen: false);

        // Skip header rows
        for (int i = 0; i < numberOfHeaderRows; i++) {
            await csvTextReader.ReadLineAsync();
        }

        // Read transactions using CsvHelper
        var cfg = new CsvConfiguration(czechCulture) {
            HasHeaderRecord = true,
            Delimiter = ";",
        };
        var csv = new CsvReader(csvTextReader, cfg);
        csv.Context.RegisterClassMap<TransactionInfoMap>();
        result.Items = csv.GetRecords<TransactionInfo>().ToList();

        return result;
    }

}