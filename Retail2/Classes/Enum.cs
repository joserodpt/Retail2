using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retail2.Classes
{
    public class Enum
    {
        //orderType = 0;1
        public enum AlertType { SUCESS, INFO, WARNING, ERROR, QUESTION}
        public enum IdentifierType { USER, PROFILE, PRODUCT, CATEGORY, ORDER }
        public enum OrderCheckout { CREDIT_CARD, MBWAY, CASH, PAYPAL, CHECK }
        public enum ValueInfo { LIKE, DISLIKE, ADD, REMOVE}

    }
}
