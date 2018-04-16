using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyzer
{
    public class CharAction
    {

        public string Oc;
        public string A;
        public string Sn = "";
        public string V1 = "";
        public string V2 = "";
        public string V3 = "";
        public string V4 = "";


        /// <param name="action">action we do on object we refer to</param>
        /// <param name="stringArray"> String array contains in order: senderName, value1, value2, value3, value4.</param>
        /// <param name="objectCalled"> name of the object to which we refer to</param>
        public CharAction(string objectCalled, string action, params string[] stringArray)
        {

            Oc = objectCalled;
            A = action;

            try
            {
                Sn = stringArray[0];
                V1 = stringArray[1];
                V2 = stringArray[2];
                V3 = stringArray[3];
                V4 = stringArray[4];
            }
            catch
            {
                // ignored
            }
        }
    }
}
