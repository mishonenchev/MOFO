using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOFO.DataProcessing.Contracts
{
    public interface IPdfBuilder
    {
        System.Web.Mvc.FileContentResult GetCardsPdf(List<Models.Card> cards);
    }
}
