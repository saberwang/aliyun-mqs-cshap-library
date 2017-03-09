using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AMQS
{
    public class MQSRequestException : Exception
    {
        public MQSRequestException(string message) :base(message)
        {
            
        }
    }
}
