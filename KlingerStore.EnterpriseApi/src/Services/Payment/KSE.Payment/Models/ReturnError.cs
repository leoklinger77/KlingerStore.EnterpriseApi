using System.Collections.Generic;

namespace KSE.Payment.Models
{
    public class ReturnError
    {
        public string url { get; set; }
        public string method { get; set; }
        public List<Errors> errors { get; set; } = new List<Errors>();
    }
    public class Errors
    {
        public string type { get; set; }
        public string parameter_name { get; set; }
        public string message { get; set; }
    }
}
