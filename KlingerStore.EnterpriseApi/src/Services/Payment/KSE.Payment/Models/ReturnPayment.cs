using System.Collections.Generic;

namespace KSE.Payment.Models
{
    public class ReturnPayment
    {
        public Transaction Transaction { get; set; }
        public ReturnError Error { get; set; }
    }
    
}
