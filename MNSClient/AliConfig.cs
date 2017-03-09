using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AliMNS
{
    [Serializable]
    public class AliConfig
    {
        public static string Endpoint { get; set; }
        public static string AccessKeySecret { get; set; }
        public static string AccessKey { get; set; }

    }
}
