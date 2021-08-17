using System.Collections.Generic;

namespace KSE.WebAppMvc.Models
{
    public class ErrorViewModel
    {
        public int ErroCode { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
    }

    public class ResponseResult
    {
        public string Title { get; set; }
        public int Status { get; set; }
        public ResponseErrorMessages errors { get; set; }

        public ResponseResult()
        {
            errors = new ResponseErrorMessages();
        }
    }
    public class ResponseErrorMessages
    {
        public List<string> Messagens { get; set; }

        public ResponseErrorMessages()
        {
            Messagens = new List<string>();
        }
    }
}
