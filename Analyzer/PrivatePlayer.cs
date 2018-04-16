using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyzer
{
    public class PrivatePlayer
    {
        public string N;
        public float X;
        public float Y;
        public PrivatePlayer(string name, float x, float y)
        {
            N = name;
            this.X = x;
            this.Y = y;
        }
    }
}
