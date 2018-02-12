using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UPF
{
    public class Pair<F, S>
    {
        public F First { get; set; }
        public S Second { get; set; }

        public Pair()
        {

        }

        public Pair(F first, S second)
        {
            First = first;
            Second = second;
        }
    }
}
