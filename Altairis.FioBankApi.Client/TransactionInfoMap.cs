using CsvHelper.Configuration;

namespace Altairis.FioBankApi.Client;

internal class TransactionInfoMap : ClassMap<TransactionInfo> {
    public TransactionInfoMap() {
        this.Map(m => m.Amount).Name("Objem");
        this.Map(m => m.BIC).Name("BIC");
        this.Map(m => m.Comments).Name("Komentář");
        this.Map(m => m.Currency).Name("Měna");
        this.Map(m => m.Date).Convert(x => DateOnly.FromDateTime(x.Row.GetField<DateTime>("Datum")));
        this.Map(m => m.Details).Name("Upřesnění");
        this.Map(m => m.Id).Name("ID pohybu");
        this.Map(m => m.KS).Name("KS");
        this.Map(m => m.MessageForRecipient).Name("Zpráva pro příjemce");
        this.Map(m => m.OrderId).Name("ID pokynu");
        this.Map(m => m.OtherAccountBankCode).Name("Kód banky");
        this.Map(m => m.OtherAccountBankName).Name("Název banky");
        this.Map(m => m.OtherAccountName).Name("Název protiúčtu");
        this.Map(m => m.OtherAccountNumber).Name("Protiúčet");
        this.Map(m => m.Person).Name("Provedl");
        this.Map(m => m.SS).Name("SS");
        this.Map(m => m.Type).Name("Typ");
        this.Map(m => m.UserIdentification).Name("Uživatelská identifikace");
        this.Map(m => m.VS).Name("VS");
    }
}
