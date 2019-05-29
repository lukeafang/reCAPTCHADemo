using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace reCAPTCHADemo.Models
{
    public class CustomRequest
    {
        public int requestID { get; set; }
        public string grecaptchaResponse { get; set; }


        public bool isSuccess { get; set; }

    }
}