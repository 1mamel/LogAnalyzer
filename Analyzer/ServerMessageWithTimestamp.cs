using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Analyzer
{
    class ServerMessageWithTimestamp
    {
        public ServerMessage ServerMessage { get; set; }
        public DateTime DateTime { get; set; }
        public String G { get; set; }
        public Brush BackgroundBrush { get; set; }

        public ServerMessageWithTimestamp(ServerMessage serverMessage, DateTime dateTime)
        {
            ServerMessage = serverMessage;
            DateTime = dateTime;
            foreach (var guid in ServerMessage.G)
            {
                G += guid.ToString() + " ";
            }
            
            BackgroundBrush = Brushes.White;
        }

    }
}
