using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AliMNS
{
    public class MNSRequestException : Exception
    {
        public MNSRequestException(string message) :base(message)
        {
            
        }
    }
}
