using System.Runtime.Serialization;

namespace BloomSales.Data.Entities
{
    [DataContract]
    public enum PaymentType
    {
        CreditCard,
        OnlineBanking,
        PayPal,
        BitCoin
    }
}