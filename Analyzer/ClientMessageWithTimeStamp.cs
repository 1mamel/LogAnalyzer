using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace Analyzer
{
     class ClientMessageWithTimeStamp 
    {
        public ClientMessage ClientMessage { get; set; }
        public DateTime DateTime { get; set; } 
        public Guid g { get; set; }
        public Brush BackgroundBrush { get; set; }
        public int Height { get; set; }
        public ClientMessageWithTimeStamp(ClientMessage clientMessage, DateTime dateTime)
        {
            ClientMessage = clientMessage;
            DateTime = dateTime;
            g = ClientMessage.g;
            BackgroundBrush = Brushes.White;
            Height = 18;
        }
        public override string ToString()
        {
            // choose any format that suits you and display what you like
            return String.Format("{0}", this.ClientMessage.g);
        }
    }
}
