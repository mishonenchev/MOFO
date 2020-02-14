using MOFO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOFO.Controllers.Contracts
{
    public interface IEmailController
    {
        void ForgotPasswordEmail(ApplicationUser user, MOFO.Models.Emails.ForgotPasswordViewModel viewModel);
    }
}
