using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOFO.Models.Emails
{
    public class ForgotPasswordViewModel
    {
        public string Callback { get; set; }
        public string Name { get; set; }
    }
}