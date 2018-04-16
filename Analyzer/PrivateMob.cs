using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyzer
{
    public class PrivateMob
    {
        /// <param name="n"> name</param>

        public string N;
        public float X;
        public float Y;
        public int Hp;
        public int T;

        public PrivateMob(string name, float x, float y, int t, int healthPoints)
        {
            T = t;
            N = name;
            X = x;
            Y = y;
            Hp = healthPoints;
        }
    }
}
