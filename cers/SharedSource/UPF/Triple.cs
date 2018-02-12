using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UPF
{
    public class Triple<F, S, T>
    {

        public F First { get; set; }
        public S Second { get; set; }
        public T Third { get; set; }

        public Triple()
        {

        }

        public Triple(F first, S second, T third)
        {
            First = first;
            Second = second;
            Third = third;
        }
    }
}
