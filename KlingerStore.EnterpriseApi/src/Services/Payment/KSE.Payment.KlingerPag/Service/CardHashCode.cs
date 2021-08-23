using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace KSE.Payment.KlingerPag.Service
{
    public class CardHashCode
    {
        public CardHashCode(KlingerPagService klingerPagService)
        {
            KlingerPagService = klingerPagService;
        }

        private readonly KlingerPagService KlingerPagService;        

        public string CardHolderName { get; set; }
        public string CardNumber { get; set; }
        public string CardExpirationDate { get; set; }
        public string CardCvv { get; set; }

        public async Task<string> GenerateAsync()
        {
            try
            {
                using var aesAlg = Aes.Create();

                aesAlg.IV = Encoding.Default.GetBytes(KlingerPagService.EncryptionKey);
                aesAlg.Key = Encoding.Default.GetBytes(KlingerPagService.ApiKey);

                var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using var msEncrypt = new MemoryStream();
                using var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);

                using (var swEncrypt = new StreamWriter(csEncrypt))
                {
                    swEncrypt.Write(CardHolderName + CardNumber + CardExpirationDate + CardCvv);
                }

                return Encoding.ASCII.GetString(msEncrypt.ToArray());

            }
            catch (System.Exception e)
            {
                throw;
            }
            
        }
    }
}
