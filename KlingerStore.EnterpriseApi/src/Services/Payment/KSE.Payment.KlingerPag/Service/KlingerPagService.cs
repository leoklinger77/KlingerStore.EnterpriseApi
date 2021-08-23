namespace KSE.Payment.KlingerPag.Service
{
    public class KlingerPagService
    {
        public readonly string ApiKey;
        public readonly string EncryptionKey;

        public KlingerPagService(string apiKey, string encryptionKey)
        {
            ApiKey = apiKey;
            EncryptionKey = encryptionKey;
        }
    }
}
