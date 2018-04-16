using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyzer
{
    class ClientMessage
    {
#pragma warning disable CS0649
        public float X;
        public float Y;
        public string MN;
        public List<PrivateMob> ML;
        public List<CharAction> AL;
        public Guid g;

#pragma warning restore CS0649
    }
}
