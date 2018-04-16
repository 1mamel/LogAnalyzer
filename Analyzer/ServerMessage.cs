using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyzer
{
    class ServerMessage
    {
        public bool IM;
        public string MN;
        public List<PrivatePlayer> PL;
        public List<PrivateMob> ML;
        public List<CharAction> AL;
        public List<Guid> G;
        public ServerMessage(bool isMaster, string mapName, List<PrivatePlayer> playerList, List<PrivateMob> mobList, List<CharAction> actionList, List<Guid> guidList)
        {
            IM = isMaster;
            MN = mapName;
            PL = playerList;
            ML = mobList;
            AL = actionList;
            G = guidList;
        }
    }
}
