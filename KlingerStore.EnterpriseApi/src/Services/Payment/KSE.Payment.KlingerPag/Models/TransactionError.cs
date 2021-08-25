using System.Collections.Generic;

namespace KSE.Payment.KlingerPag.Models
{
    public class TransactionError
    {
        public string url { get; set; }
        public string method { get; set; }
        public List<Errors> errors { get; set; }
    }
    public class Errors
    {
        public string type { get; set; }
        public string parameter_name { get; set; }
        public string message { get; set; }
    }
}
