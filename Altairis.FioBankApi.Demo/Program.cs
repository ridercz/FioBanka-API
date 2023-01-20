using Altairis.FioBankApi.Client;

Console.WriteLine("Altairis FIO Bank API Client demo application");
Console.WriteLine("Copyright (c) Michal A. Valášek - Altairis, 2023");
Console.WriteLine("https://www.github.com/ridercz/FioBanka-API");
Console.WriteLine();

if (args.Length != 1) {
    Console.WriteLine("Usage: fiodemo <api access token>");
    Environment.Exit(1);
}

Console.Write("Creating API client instance...");
var fio = new FioClient(args[0]);
Console.WriteLine("OK");
Console.WriteLine();

Console.Write("Getting transactions from last 7 days...");
await PerformSafeApiCall(() => fio.GetTransactions(DateOnly.FromDateTime(DateTime.Today.AddDays(-7))));
Wait();
Console.WriteLine();

Console.Write("Getting transactions since cursor (should be empty)...");
await PerformSafeApiCall(() => fio.GetTransactions());
Wait();
Console.WriteLine();

Console.Write("Setting cursor 7 days back...");
await fio.SetCursor(DateOnly.FromDateTime(DateTime.Today.AddDays(-7)));
Console.WriteLine("OK");
Console.WriteLine();

Console.Write("Getting transactions since cursor again...");
await PerformSafeApiCall(() => fio.GetTransactions());

async Task PerformSafeApiCall(Func<Task<TransactionList>> call) {
    try {
        var tl = await call();
        Console.WriteLine($"OK, account IBAN {tl.Iban}");
        PrintTransactionList(tl);
    } catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Conflict) {
        Console.WriteLine("Failed!");
        Console.WriteLine("Too many requests - API supports only one request per 30 seconds.");
        Environment.Exit(2);
    }
}

void Wait() {
    Console.Write("Waiting for 30 seconds due to API rate limiting");
    for (var i = 0; i < 30; i++) {
        Console.Write('.');
        Thread.Sleep(1000);
    }
    Console.WriteLine("OK");
}

void PrintTransactionList(TransactionList tl) {
    if (tl.Items.Any()) {
        Console.WriteLine($"Transactions from {tl.DateStart} to {tl.DateEnd}:");
        Console.WriteLine(new string('-', Console.WindowWidth));
        Console.WriteLine("Date       |         Amount | VS         | Other account number (name)");
        Console.WriteLine(new string('-', Console.WindowWidth));
        foreach (var item in tl.Items) {
            Console.WriteLine($"{item.Date:dd.MM.yyyy} | {item.Amount,10} {item.Currency} | {item.VS,-10} | {item.OtherAccountNumber}/{item.OtherAccountBankCode} ({item.OtherAccountName})");
        }
        Console.WriteLine(new string('-', Console.WindowWidth));
    } else {
        Console.WriteLine($"No transaction available in the requested period from {tl.DateStart} to {tl.DateEnd}.");
    }
}