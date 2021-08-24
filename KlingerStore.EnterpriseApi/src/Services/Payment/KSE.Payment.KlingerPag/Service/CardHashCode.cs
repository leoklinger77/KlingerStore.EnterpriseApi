using PagarMe;
using System.Threading.Tasks;

namespace KSE.Payment.KlingerPag.Service
{
    public class CardHashCode
    {
        private readonly string DefaultEncryptionKey;
        public string CardHolderName { get; set; }
        public string CardNumber { get; set; }
        public string CardExpirationDate { get; set; }
        public string CardCvv { get; set; }

        public CardHashCode(string defaultEncryptionKey)
        {
            DefaultEncryptionKey = defaultEncryptionKey;
        }                

        public async Task<string> GenerateAsync()
        {
            PagarMeService.DefaultEncryptionKey = DefaultEncryptionKey;

            CardHash card = new CardHash();

            card.CardNumber = CardNumber;
            card.CardHolderName = CardHolderName;
            card.CardExpirationDate = CardExpirationDate;
            card.CardCvv = CardCvv;

            string cardhash = card.Generate();

            return cardhash;
        }
    }
}
