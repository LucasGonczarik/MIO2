using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MIO2
{
    class Dendrite
    {
        public double Weight { get; set; }

        //Provide a constructor for the class.
        //It is always better to provide a constructor instead of using
        //the compiler provided constructor
        public Dendrite()
        {
            Weight = getRandom(0.00000001, 1.0);
        }

        private double getRandom(double MinValue, double MaxValue)
        {
            Random random = new Random();
            return random.NextDouble() * (MaxValue - MinValue) + MinValue;
        }
    }
}
