namespace PruebaTecnicaVueling.Models
{
    public class TransactionsSku
    {
        public string Sku { get; set; }

        public string Currency { get; set; }

        public decimal TotalAmount { get; set; }
        public uint TotalErrors { get; set; }

        public decimal[] TransactionAmounts { get; set; }

    }
}
